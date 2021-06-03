using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class TransferList : AntDomComponentBase
    {
        [CascadingParameter]
        private Transfer Transfer { get; set; }

        [Parameter]
        public TransferDirection Direction { get; set; }

        [Parameter]
        public string TitleText { get; set; }

        [Parameter]
        public bool ShowSelectAll { get; set; }

        [Parameter]
        public string ItemUnit { get; set; }

        [Parameter]
        public string ItemsUnit { get; set; }

        [Parameter]
        public string Filter { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool ShowSearch { get; set; }

        [Parameter]
        public string SearchPlaceholder { get; set; }

        [Parameter]
        public string NotFoundContent { get; set; }

        /// <summary>
        /// (inputValue: string, item: TransferItem) => boolean
        /// </summary>
        [Parameter]
        public Func<string, TransferItem, bool> FilterOption { get; set; }

        [Parameter]
        public IEnumerable<TransferItem> DataSource { get; set; }

        [Parameter]
        public RenderFragment RenderList { get; set; }

        [Parameter]
        public RenderFragment<TransferItem> Render { get; set; }

        [Parameter]
        public string Footer { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public EventCallback<bool> OnHandleSelectAll { get; set; }

        [Parameter]
        public EventCallback<TransferItem> OnSelect { get; set; }

        [Parameter]
        public EventCallback<(string direction, string value)> FilterChange { get; set; }

        private const string DisabledClass = "ant-transfer-list-content-item-disabled";
        private const string FooterClass = "ant-transfer-list-with-footer";

        private string _filterValue;

        private List<string> _sourceSelectedKeys;
        private List<string> _targetSelectedKeys;

        private string _leftFilterValue = string.Empty;
        private string _rightFilterValue = string.Empty;

        private string _leftCountText = string.Empty;
        private string _rightCountText = string.Empty;

        private bool _leftButtonDisabled = true;
        private bool _rightButtonDisabled = true;

        private IEnumerable<TransferItem> _leftDataSource;
        private IEnumerable<TransferItem> _rightDataSource;

        private List<string> _targetKeys;

        private bool _leftCheckAllState;
        private bool _leftCheckAllIndeterminate;
        private bool _rightCheckAllState;
        private bool _rightCheckAllIndeterminate;

        private record Stat(bool CheckAll, bool CheckHalf, int CheckCount, int ShownCount);

        private Stat _stat = new(false, false, 0, 0);

        private async Task SelectItem(bool isCheck, TransferDirection direction, string key)
        {
            var holder = direction == TransferDirection.Left ? _sourceSelectedKeys : _targetSelectedKeys;
            var index = Array.IndexOf(holder.ToArray(), key);

            if (index > -1)
            {
                holder.RemoveAt(index);
            }
            if (isCheck)
                holder.Add(key);

            HandleSelect(direction, holder);

            //if (OnSelectChange.HasDelegate)
            //{
            //    await OnSelectChange.InvokeAsync(new TransferSelectChangeArgs(_sourceSelectedKeys.ToArray(), _targetSelectedKeys.ToArray()));
            //}
        }

        private async Task SelectAll(bool isCheck, TransferDirection direction)
        {
            var list = _leftDataSource;
            if (direction == TransferDirection.Right)
            {
                list = _rightDataSource;
            }

            var holder = isCheck ? list.Where(a => !a.Disabled).Select(a => a.Key).ToList() : new List<string>(list.Count());

            HandleSelect(direction, holder);

            //if (OnSelectChange.HasDelegate)
            //{
            //    await OnSelectChange.InvokeAsync(new TransferSelectChangeArgs(_sourceSelectedKeys.ToArray(), _targetSelectedKeys.ToArray()));
            //}
        }

        private void HandleSelect(TransferDirection direction, List<string> keys)
        {
            if (direction == TransferDirection.Left)
            {
                _sourceSelectedKeys = keys;
            }
            else
            {
                _targetSelectedKeys = keys;
            }
        }

        private async Task HandleSearch(ChangeEventArgs e, TransferDirection direction, bool mathTileCount = true)
        {
            _filterValue = e.Value.ToString();
            DataSource = DataSource.Where(a => !_targetKeys.Contains(a.Key) && a.Title.Contains(_leftFilterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (mathTileCount)
                MathTitleCount();

            //if (OnSearch.HasDelegate)
            //{
            //    await OnSearch.InvokeAsync(new TransferSearchArgs(direction, e.Value.ToString()));
            //}
        }

        private async Task ClearFilterValueAsync(TransferDirection direction)
        {
            if (direction == TransferDirection.Left)
            {
                _leftFilterValue = string.Empty;
                await HandleSearch(new ChangeEventArgs() { Value = string.Empty }, direction);
            }
            else
            {
                _rightFilterValue = string.Empty;
                await HandleSearch(new ChangeEventArgs() { Value = string.Empty }, direction);
            }
        }

        private void MathTitleCount()
        {
            _rightButtonDisabled = _sourceSelectedKeys.Count == 0;
            _leftButtonDisabled = _targetSelectedKeys.Count == 0;

            var leftSuffix = _leftDataSource.Count() == 1 ? Transfer?.Locale.ItemUnit : Transfer?.Locale.ItemsUnit;
            var rightSuffix = _rightDataSource.Count() == 1 ? Transfer?.Locale.ItemUnit : Transfer?.Locale.ItemsUnit;

            var leftCount = _sourceSelectedKeys.Count == 0 ? $"{_leftDataSource.Count()}" : $"{_sourceSelectedKeys.Count}/{_leftDataSource.Count()}";
            var rightCount = _targetSelectedKeys.Count == 0 ? $"{_rightDataSource.Count()}" : $"{_targetSelectedKeys.Count}/{_rightDataSource.Count()}";

            _leftCountText = $"{leftCount} {leftSuffix}";
            _rightCountText = $"{rightCount} {rightSuffix}";

            CheckAllState();
        }

        private void CheckAllState()
        {
            if (_leftDataSource.Any(a => !a.Disabled))
            {
                _leftCheckAllState = _sourceSelectedKeys.Count == _leftDataSource.Count(a => !a.Disabled);
            }
            else
            {
                _leftCheckAllState = false;
            }

            _leftCheckAllIndeterminate = !_leftCheckAllState && _sourceSelectedKeys.Count > 0;

            if (_rightDataSource.Any(a => !a.Disabled))
            {
                _rightCheckAllState = _targetSelectedKeys.Count == _rightDataSource.Count(a => !a.Disabled);
            }
            else
            {
                _rightCheckAllState = false;
            }

            _rightCheckAllIndeterminate = !_rightCheckAllState && _targetSelectedKeys.Count > 0;
        }
    }
}
