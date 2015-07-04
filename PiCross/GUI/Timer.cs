using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class Timer
    {
        private DateTime lastTick;

        public Timer()
        {
            ResetLastTick();
        }

        public void ResetLastTick()
        {
            lastTick = DateTime.Now;
        }

        public void OnTick()
        {
            var now = DateTime.Now;
            var delta = ( now - lastTick ).TotalSeconds;
            lastTick = now;

            if ( Tick != null )
            {
                Tick( delta );
            }
        }

        public event Action<double> Tick;
    }
}
