using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Config;
using Const;
using Models;
using Utils;

namespace Services
{
    public class SimulationEngine
    {
        private Tile[,] board = new Tile[BoardConfig.height, BoardConfig.width];
        private List<Colony> colonies = new List<Colony>();

        public void init_board()
        {
            for (int i = 0; i < BoardConfig.height; i++)
            {
                for (int j = 0; j < BoardConfig.width; j++)
                {
                    board[i, j] = new Tile();
                }
            }

            foreach (Pos pos in SimUtils.GenerateUniquePoses(GameConfig.initColoniesCount, BoardConfig.height, BoardConfig.width, GameConfig.randomSeed))
            {
                Colony colony = new Colony();
                colonies.Add(colony);

                board[pos.x, pos.y] = new Cell(colony, GameConfig.generationEnergy);
            }
        }
        public void run()
        {
            while (true)
            {
                for (int i = 0; i < GameConfig.stepPeriod; i++)
                {
                    update();
                }
                draw_state();
            }
        }
        public void update()
        {
            ActionType action;
            Tile tile, tile_;
            Cell cell;
            CellViewObject cellViewObject;
            Pos shift;
            for (int i = 0; i < BoardConfig.height; i++)
            {
                for (int j = 0; j < BoardConfig.width; j++)
                {
                    tile = board[i, j];
                    if (tile is Cell)
                    {
                        cell = tile as Cell;
                        shift = SimUtils.getShift(cell.getOrientation());
                        cellViewObject = getViewObject(i, j, shift);

                        action = cell.step(cellViewObject);
                        if (cell.isAlive())
                        {

                        }
                        else
                        {
                            board[i, j] = new Food(GameConfig.afterCellFoodAmount);
                        }

                    }
                    else
                    {
                        tile.step();

                        if (tile is Food && ((Food)tile).getEnergy() <= 0)
                        {
                            board[i, j] = new Tile();
                        }
                    }
                }
            }
        }

        public CellViewObject getViewObject(int x, int y, Pos shift)
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
                    if (((Cell)tile).colonyID == ((Cell)tile).colonyID)
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

        public void draw_state()
        {
            Dictionary<String, object> simData = new Dictionary<string, object>();
            simData["height"] = BoardConfig.height;
            simData["width"] = BoardConfig.width;

            simData["board"] = new List<Dictionary<String, object>>();
            for (int i = 0; i < BoardConfig.height; i++)
            {
                for (int j = 0; j < BoardConfig.width; j++)
                {
                    if (!(board[i, j] is Tile))
                    {
                        simData["board"].Add(board[i, j].Serialize(j, i));
                    }
                }
            }
        }
    }
}