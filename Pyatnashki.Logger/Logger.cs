using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Pyatnashki.Logger
{
    public class Logger
    {
        public static readonly NLog.Logger Instance = LogManager.GetCurrentClassLogger();
    }
}
