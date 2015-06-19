﻿using System;
using PiCross.Cells;

namespace GUI.Commands
{
    public abstract class EnabledCommand : CellCommand
    {
        public static EnabledCommand FromDelegate( Action action )
        {
            return new ActionEnabledCommand( ( parameter ) => action() );
        }

        public static EnabledCommand FromDelegate( Action<object> action )
        {
            return new ActionEnabledCommand( action );
        }

        protected EnabledCommand()
            : base( Cell.Create( true ) )
        {
            // NOP
        }

        private class ActionEnabledCommand : EnabledCommand
        {
            private readonly Action<object> action;

            public ActionEnabledCommand( Action<object> action )
            {
                if ( action == null )
                {
                    throw new ArgumentNullException( "action" );
                }
                else
                {
                    this.action = action;
                }
            }

            public override void Execute( object parameter )
            {
                action( parameter );
            }
        }
    }
}
