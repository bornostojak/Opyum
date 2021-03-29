using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures
{
    interface ICommand<T>
    {
        void Do(T data);

        void Undo();

        void Redo();
    }
}
