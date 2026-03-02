using Pinger3.ViewModels.Controls;
using System;

namespace Pinger3.ViewModels.Design
{
    internal class PingingTargetWrapperViewModelDesign : IPingingTargetWrapperViewModel
    {
        private bool isMarkedForPopout;

        public bool IsMarkedForPopout { get => Random.Shared.Next(0, 1) == 0; set => isMarkedForPopout = value; }

        public IPingingTargetViewModel TargetViewModel => new PingingTargetViewModelDesign();
    }
}
