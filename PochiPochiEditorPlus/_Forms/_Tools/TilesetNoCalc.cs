using System.Drawing;
using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;
using PochiPochiEditorPlus._Managers;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Forms._Tools
{
    public partial class TilesetNoCalc : Form
    {
        private EventBinder _eventBinder = null;
        private TilesetManager _tilesetManager = null;

        public TilesetNoCalc(SharedData sharedData)
        {
            InitializeComponent();

            _eventBinder = new EventBinder();
            _tilesetManager = new TilesetManager(sharedData);
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            _eventBinder.BindCtrl(
                h => btnConvNoToHeaderOffset.Click += h,
                h => btnConvNoToHeaderOffset.Click -= h,
                (_, __) =>
                {
                    int tilesetNo = (int)nudTilesetNo.Value;
                    int offset = _tilesetManager.CalcOffset(tilesetNo);
                    txtHeaderOffset.Text = offset.ParseIntToString();
                    SetSuccess();
                });
            _eventBinder.BindCtrl(
                h => btnConvHeaderOffsetToNo.Click += h,
                h => btnConvHeaderOffsetToNo.Click -= h,
                (_, __) =>
                {
                    int offset = txtHeaderOffset.Text.ParseStringToInt();

                    // 空白、16進数出ない場合
                    if (offset == Constants.InvalidValue)
                    {
                        SetFailure(0);
                        return;
                    }

                    if (_tilesetManager.TryCalcTilesetNo(offset, out int tilesetNo))
                    {
                        nudTilesetNo.Value = tilesetNo;
                        SetSuccess();
                    }
                    else
                    {
                        // 近い番号を計算
                        int recNo = _tilesetManager.CalcNearestTilesetNo(offset);
                        SetFailure(recNo);
                    }
                });

            // 解除タイミング指定
            _eventBinder.BindCtrl(
                h => this.Disposed += h,
                h => this.Disposed -= h);
        }

        private void SetSuccess()
        {
            // 成功時のUI更新
            lblConvResult.Text = "成功";
            lblConvResult.ForeColor = Color.Green;

            // 代替値をリセット
            CtrlHelper.ResetControls(grpConvResult, includeSelf: false);
        }

        private void SetFailure(int recNo)
        {
            // 失敗時のUI更新
            lblConvResult.Text = "失敗";
            lblConvResult.ForeColor = Color.Red;

            // 代替番号とオフセットを代入
            nudRecTilesetNo.Value = recNo;
            txtRecHeaderOffset.Text = 
                _tilesetManager.CalcOffset(recNo).ParseIntToString();
        }
    }
}
