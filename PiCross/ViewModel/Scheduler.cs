using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Scheduler
    {
        private double timeLeft;

        private Action action;

        public Scheduler(double time, Action action)
        {
            this.timeLeft = time;
            this.action = action;
        }

        public void Tick(double dt)
        {
            timeLeft -= dt;

            if ( timeLeft < 0 )
            {
                action();
                action = delegate() { };
            }
        }
    }
}
