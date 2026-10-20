using Const;
using Models;

namespace Services
{
    public class SimulationEngine
    {
        private Tile[,] board = new Tile[Settings.boardConfig.height, Settings.boardConfig.width];
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
            
        }
        public void draw_state()
        {
            
        }
    }
}