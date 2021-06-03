using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Transfer : AntDomComponentBase
    {
        private const string PrefixName = "ant-transfer";

        [Parameter]
        public IList<TransferItem> DataSource { get; set; }

        [Parameter]
        public string[] Titles { get; set; } = new string[2];

        [Parameter]
        public string[] Operations { get; set; } = new string[2];

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public bool ShowSearch { get; set; } = false;

        [Parameter]
        public bool ShowSelectAll { get; set; } = true;

        [Parameter]
        public string[] TargetKeys { get; set; }

        [Parameter]
        public string[] SelectedKeys { get; set; }

        [Parameter]
        public EventCallback<TransferChangeArgs> OnChange { get; set; }

        [Parameter]
        public EventCallback<TransferScrollArgs> OnScroll { get; set; }

        [Parameter]
        public EventCallback<TransferSearchArgs> OnSearch { get; set; }

        [Parameter]
        public EventCallback<TransferSelectChangeArgs> OnSelectChange { get; set; }

        [Parameter]
        public Func<TransferItem, OneOf<string, RenderFragment>> Render { get; set; }

        [Parameter]
        public TransferLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Transfer;

        [Parameter]
        public string Footer { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private List<string> _targetKeys;

        protected override void OnInitialized()
        {
            ClassMapper
                .Add(PrefixName)
                .If($"{PrefixName}-rtl", () => RTL);

            _targetKeys = TargetKeys.ToList();
            var selectedKeys = SelectedKeys.ToList();
        }

        private async Task MoveItem(MouseEventArgs e, TransferDirection direction)
        {
            //var moveKeys = direction == TransferDirection.Right ? _sourceSelectedKeys : _targetSelectedKeys;

            //if (direction == TransferDirection.Left)
            //{
            //    _targetKeys.RemoveAll(key => moveKeys.Contains(key));
            //}
            //else
            //{
            //    _targetKeys.AddRange(moveKeys);
            //}

            //var oppositeDirection = direction == TransferDirection.Right ? TransferDirection.Left : TransferDirection.Right;

            //HandleSelect(oppositeDirection, new List<string>());

            //if (!string.IsNullOrEmpty(_leftFilterValue))
            //{
            //    await HandleSearch(new ChangeEventArgs() { Value = _leftFilterValue }, TransferDirection.Left, false);
            //}

            //if (!string.IsNullOrEmpty(_rightFilterValue))
            //{
            //    await HandleSearch(new ChangeEventArgs() { Value = _rightFilterValue }, TransferDirection.Right, false);
            //}

            //if (OnChange.HasDelegate)
            //{
            //    await OnChange.InvokeAsync(new TransferChangeArgs(_targetKeys.ToArray(), direction, moveKeys.ToArray()));
            //}
        }

        //private async Task HandleScroll(string direction, EventArgs e)
        //{
        //    if (OnScroll.HasDelegate)
        //    {
        //        await OnScroll.InvokeAsync(new TransferScrollArgs(direction, e));
        //    }
        //}
    }
}
