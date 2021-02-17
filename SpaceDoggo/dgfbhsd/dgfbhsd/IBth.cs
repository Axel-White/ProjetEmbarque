using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dgfbhsd
{
    public interface IBth
    {
        void Disconnect();
        void Connect(string name);
        List<string> PairedDevices();
        Task WaitForScore();
    }
}
