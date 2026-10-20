using Const;
using Models;
using Utils;

namespace Services
{
    public class SimulationEngine
    {
        private Tile[,] board = new Tile[Settings.boardConfig.height, Settings.boardConfig.width];
        
        public void init_board()
        {
            
        }
        public void run()
        {
            while (true)
            {
                for (int i=0; i<Settings.gameConfig.stepPeriod; i++)
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
            for (int i=0; i<Settings.boardConfig.height; i++)
            {
                for (int j=0; j<Settings.boardConfig.width; j++)
                {
                    tile = board[i,j];
                    if (tile is Cell)
                    {
                        cell = tile as Cell;
                        shift = SimUtils.getShift(cell.getOrientation());
                        cellViewObject = getViewObject(i, j, shift);

                        action = cell.step(cellViewObject);
                        if (cell.isAlive())
                        {
                            
                        } else
                        {
                            board[i,j] = new Food(Settings.gameConfig.afterCellFoodAmount);
                        }

                    } else
                    {
                        tile.step();

                        if (tile is Food && ((Food)tile).getEnergy() <= 0)
                        {
                            board[i,j] = new Tile();
                        }
                    }        
                } 
            }
        }

        public CellViewObject getViewObject(int x, int y, Pos shift)
        {
            CellViewObject cellViewObject;
            Tile tile;

            if (y+shift.y < 0 || x+shift.x < 0 || y+shift.y >= Settings.boardConfig.height || x+shift.x >= Settings.boardConfig.width)
            {
                cellViewObject = CellViewObject.WALL;
            } else
            {
                tile = board[y+shift.y, x+shift.x];
                if (tile is Food)
                {
                    cellViewObject = CellViewObject.FOOD;
                } else if(tile is Cell)
                {
                    if (((Cell)tile).colonyID == ((Cell)tile).colonyID)
                    {
                        cellViewObject = CellViewObject.ALLIE;
                    } else
                {
                        cellViewObject = CellViewObject.ENEMY;
                }
                } else
                {
                    cellViewObject = CellViewObject.NONE;
                }
            }

            return cellViewObject;
        }

        public void draw_state()
        {
            
        }
    }
}