using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

using Config;
using Const;
using Models;
using Utils;
using System.Text.Json;

namespace Services
{
    public class SimulationEngine
    {
        private int time = 0;
        private string logDir=DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        private Tile[,] board = new Tile[BoardConfig.height, BoardConfig.width];
        private List<Colony> colonies = new List<Colony>();

        public void InitDirs()
        {
            if(!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
            if (!Directory.Exists($"Logs/{logDir}")) Directory.CreateDirectory($"Logs/{logDir}");
            if(!Directory.Exists("Videos")) Directory.CreateDirectory("Videos");
        }

        public void InitBoard()
        {
            for (int i = 0; i < BoardConfig.height; i++)
            {
                for (int j = 0; j < BoardConfig.width; j++)
                {
                    board[i, j] = new Tile();
                }
            }

            foreach (Pos pos in SimUtils.GenerateUniquePoses(GameConfig.initColoniesCount, BoardConfig.height, BoardConfig.width))
            {
                Colony colony = new Colony();
                colonies.Add(colony);

                board[pos.y, pos.x] = new Cell(colony, GameConfig.generationEnergy);
            }
        }



        public void Run()
        {
            InitDirs();
            InitBoard();
            Constants.videoRecorder.InitVideoRecorder();

            while (time < GameConfig.simIterations)
            {
                bool status = true;
                for (int i = 0; i < GameConfig.stepPeriod; i++)
                {
                    status = Update() != 0;
                }
                time++;
                Draw_state();

                if (time % GameConfig.saveFreq == 0)
                {
                    CreateLogDir();
                    SaveJSONState();
                    SaveConfig();
                }

                if (!status) break;
                Console.WriteLine(time);
            }

            Constants.videoRecorder.CloseVideoRecorder();
        }
        public int Update()
        {
            ActionType action;
            Tile tile, tile_;
            Cell cell;
            CellViewObject cellViewObject;
            Pos shift;
            int cellSteps=0;
            for (int i = 0; i < BoardConfig.height; i++)
            {
                for (int j = 0; j < BoardConfig.width; j++)
                {
                    tile = board[i, j];
                    if (tile is Cell)
                    {
                        cell = (Cell)tile;
                        shift = SimUtils.GetShift(cell.Orientation);
                        cellViewObject = GetViewObject(i, j, shift);
    
                        action = cell.Step(cellViewObject);
                        if (cell.IsAlive)
                        {
                            cellSteps++;
                            ApplyCellAction(i, j, action);
                            if (!cell.IsAlive)
                            {
                                cell.colony.localRelatives--;
                                //if (cell.colony.ancestorID != null) cell.colony.ancestor!.globalRelatives--;
                                if (cell.energy > 0) board[i, j] = new Food(cell.energy);
                                else board[i, j] = new Tile();
                            }
                        }
                        else
                        {
                            cell.colony.localRelatives--;
                            //if (cell.colony.ancestorID != null) cell.colony.ancestor!.globalRelatives--;
                            if (cell.energy > 0) board[i, j] = new Food(cell.energy);
                            else board[i, j] = new Tile();
                        }

                    }
                    else
                    {
                        tile.Step();

                        if (tile is Food && ((Food)tile).GetEnergy() <= 0)
                        {
                            board[i, j] = new Tile();
                        } else if (Constants.random.NextDouble() <= GameConfig.foodSpawnChance)
                        {
                            board[i, j] = new Food(GameConfig.foodSpawnEnergyAmount);
                        }
                    }
                }
            }

            return cellSteps;
        }

        public void ApplyCellAction(int y, int x, ActionType action)
        {
            Cell selectedCell = (Cell)board[y,x];
            Tile? tile;
            Pos shift;
            //selectedCell.DecreaseEnergy((1-(1/selectedCell.age))*GameConfig.maxLiveEnergyCost);
            switch (action)
            {
                case ActionType.ATTACK:
                    selectedCell.DecreaseEnergy(GameConfig.attackEnergyCost);
                    shift = SimUtils.GetShift(selectedCell.Orientation);
                    tile = GetViewTile(y, x, shift);
                    if (tile is Cell)
                    {
                        Cell attackedCell = (Cell)tile;
                        attackedCell.DecreaseHp(GameConfig.damage);
                        if (!attackedCell.IsAlive)
                        {
                            attackedCell.colony.localRelatives--;
                            //if (attackedCell.colony.ancestorID != null) attackedCell.colony.ancestor!.globalRelatives--;
                            if (attackedCell.energy > 0) board[y+shift.y, x+shift.x] = new Food(attackedCell.energy);
                            else board[y, x] = new Tile();
                        }
                    }

                    break;
                case ActionType.MOVE:
                    selectedCell.DecreaseEnergy(GameConfig.moveEnergyCost);
                    shift = SimUtils.GetShift(selectedCell.Orientation);
                    tile = GetViewTile(y, x, shift);
                    if (tile == null || tile is Cell) break;
                    
                    if (tile is Food)
                    {
                        selectedCell.IncreaseEnergy(((Food)tile).GetEnergy());
                    }
                    board[y,x] = new Tile();
                    board[y+shift.y, x+shift.x] = selectedCell;

                    break;
                case ActionType.LEFT:
                    switch (selectedCell.Orientation)
                    {
                        case Orientation.FORWARD:
                            selectedCell.Orientation = Orientation.LEFT;
                            break;
                        case Orientation.LEFT:
                            selectedCell.Orientation = Orientation.BACKWARD;
                            break;
                        case Orientation.RIGHT:
                            selectedCell.Orientation = Orientation.FORWARD;
                            break;
                        case Orientation.BACKWARD:
                            selectedCell.Orientation = Orientation.RIGHT;
                            break;
                    }
                    break;
                case ActionType.RIGHT:
                    switch (selectedCell.Orientation)
                    {
                        case Orientation.FORWARD:
                            selectedCell.Orientation = Orientation.RIGHT;
                            break;
                        case Orientation.LEFT:
                            selectedCell.Orientation = Orientation.FORWARD;
                            break;
                        case Orientation.RIGHT:
                            selectedCell.Orientation = Orientation.BACKWARD;
                            break;
                        case Orientation.BACKWARD:
                            selectedCell.Orientation = Orientation.LEFT;
                            break;
                    }
                    break;
                case ActionType.DIVIDE:
                    Colony colony;
                    Pos divPos = FindDivisionPose(y, x, selectedCell.Orientation);
                    if (divPos.x == -1) break;
                    
                    if (Constants.random.NextDouble() > GameConfig.genomeMutationChance)
                    {
                        colony = selectedCell.colony;
                    } else
                    {
                        colony = selectedCell.colony.GenerateNewColony();
                        colonies.Add(colony);
                    }
                    
                    colony.localRelatives++;
                    //if (colony.ancestorID != null) colony.ancestor!.globalRelatives++;

                    board[divPos.y, divPos.x] = new Cell(colony, selectedCell.energy/2);
                    selectedCell.DivideEnergy();

                    break;
                case ActionType.HEAL:
                    selectedCell.Heal();
                    break;
                case ActionType.NONE:
                    break;
            }
        }

        public Pos FindDivisionPose(int y, int x, Orientation orientation)
        {
            Pos shift, newPos;

            for (int i=0; i<4; i++)
            {
                shift = SimUtils.GetShift((Orientation)(((int)orientation+i)%4));
                newPos = new Pos{x=x+shift.x, y=y+shift.y};
                
                if (BoardConfig.height>newPos.y && newPos.y>=0 && BoardConfig.width>newPos.x && newPos.x>=0 && !(board[newPos.y,newPos.x] is Cell))
                {
                    return newPos;                    
                }
            }

            return new Pos{x=-1, y=-1};
        }

        public Tile? GetViewTile(int y, int x, Pos shift)
        {
            if (y + shift.y < 0 || x + shift.x < 0 || y + shift.y >= BoardConfig.height || x + shift.x >= BoardConfig.width)
            {
                return null;
            }

            return board[y+shift.y, x+shift.x];
        }

        public CellViewObject GetViewObject(int y, int x, Pos shift)
        {
            CellViewObject cellViewObject;
            Tile tile;

            if (y + shift.y < 0 || x + shift.x < 0 || y + shift.y >= BoardConfig.height || x + shift.x >= BoardConfig.width)
            {
                cellViewObject = CellViewObject.WALL;
            }
            else
            {
                tile = board[y + shift.y, x + shift.x];
                if (tile is Food)
                {
                    cellViewObject = CellViewObject.FOOD;
                }
                else if (tile is Cell)
                {
                    if (((Cell)board[y,x]).colonyID == ((Cell)tile).colonyID)
                    {
                        cellViewObject = CellViewObject.ALLIE;
                    }
                    else
                    {
                        cellViewObject = CellViewObject.ENEMY;
                    }
                }
                else
                {
                    cellViewObject = CellViewObject.NONE;
                }
            }

            return cellViewObject;
        }

        public void Draw_state()
        {
            if (Constants.videoRecorder.CheckStatus()) return;

            int videoWidth = BoardConfig.width * VideoConfig.cellSize;
            int videoHeight = BoardConfig.height * VideoConfig.cellSize;

            using (Image<Rgb24> frameImage = new Image<Rgb24>(videoWidth, videoHeight))
            {
                frameImage.Mutate(ctx =>
                {
                    ctx.Clear(Color.Black);

                    for (int y = 0; y < BoardConfig.height; y++)
                    {
                        for (int x = 0; x < BoardConfig.width; x++)
                        {
                            Tile tile = board[y, x];
                            Color cellColor = Color.Black;

                            if (tile is Cell cell)
                            {
                                int hash = cell.colony.GetHashCode();
                                cellColor = Color.FromRgb((byte)(hash & 0xFF), (byte)((hash >> 8) & 0xFF), (byte)((hash >> 16) & 0xFF));
                            }
                            else if (tile is Food)
                            {
                                cellColor = Color.Green;
                            }
                            else if (tile is Tile)
                            {
                                continue;
                            }

                            if (cellColor != Color.Black)
                            {
                                var rect = new RectangleF(x * VideoConfig.cellSize, y * VideoConfig.cellSize, VideoConfig.cellSize, VideoConfig.cellSize);
                                ctx.Fill(cellColor, rect);
                            }
                        }
                    }
                });

                //frameImage.Save("image.png");
                Constants.videoRecorder.CopyToBuffer(frameImage);
            }

            Constants.videoRecorder.SaveBuffer();
        }
        public void CreateLogDir()
        {
            Directory.CreateDirectory($"Logs/{logDir}/Evolution_{time}");
        }
        public void SaveJSONState()
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true 
            };
            
            Dictionary<String, object> simData = new Dictionary<string, object>();
            simData["height"] = BoardConfig.height;
            simData["width"] = BoardConfig.width;

            List<Dictionary<String, object>> boardObjects = new List<Dictionary<String, object>>();
            for (int i = 0; i < BoardConfig.height; i++)
            {
                for (int j = 0; j < BoardConfig.width; j++)
                {
                    if (board[i, j] is Cell || board[i, j] is Food)
                    {
                        boardObjects.Add(board[i,j].ToJson(i, j));
                    }
                }
            }
            simData["board"] = boardObjects;

            List<Dictionary<String, object>> simColonies = new List<Dictionary<String, object>>();
            foreach (var colony in colonies)
            {
                if (colony.IsAlive) simColonies.Add(colony.ToJson());
            }
            simData["colonies"] = simColonies;

            File.WriteAllText($"Logs/{logDir}/Evolution_{time}/board_{time}.json", JsonSerializer.Serialize(simData));
            //File.WriteAllText($"Logs/{logDir}/Evolution_{time}/board_{time}.json", JsonSerializer.Serialize(simData, options));
        }

        public void SaveConfig()
        {
            
        }
    }
}