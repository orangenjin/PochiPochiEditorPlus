using System.Windows.Forms;
using PochiPochiEditorPlus._Helpers;

namespace PochiPochiEditorPlus._Utilities._QuickInput
{
    public partial class QuickInputForm : Form
    {
        // 結果を格納する
        public QuickInputResult Result { get; set; }

        private QuickInputConfig _config = null;
        private EventBinder _eventBinder = null;

        public QuickInputForm(QuickInputConfig config)
        {
            InitializeComponent();
            _config = config;
            _eventBinder = new EventBinder();
            Result = new QuickInputResult();

            InitializeControls();
            InitializeEventHandlers();
        }

        private void InitializeControls()
        {
            // 一括無効化
            CtrlHelper.SetControlsEnabled(grpInputData, enabled: false, includeSelf: false);

            // 各コントロールを有効化
            if (_config.HasOffset)
            {
                lblResultOffset.Enabled = true;
                txtResultOffset.Enabled = true;
                txtResultOffset.Text = 
                    _config.DefaultOffset.Value.ParseIntToString();
            }

            if (_config.HasCombo)
            {
                lblResultIndex.Enabled = true;
                cmbResultIndex.Enabled = true;

                cmbResultIndex.BeginUpdate();
                cmbResultIndex.Items.Clear();
                cmbResultIndex.Items.AddRange(_config.CmbItems);
                cmbResultIndex.EndUpdate();
                cmbResultIndex.SelectedIndex = 0;
            }

            if (_config.HasNumeric)
            {
                lblResultCount.Enabled = true;
                nudResultCount.Enabled = true;
                nudResultCount.Minimum = _config.NudMin.Value;
                nudResultCount.Maximum = _config.NudMax.Value;
                nudResultCount.Value = _config.NudMin.Value;
            }

            if (_config.HasFile)
            {
                lblResultPath.Enabled = true;
                txtResultPath.Enabled = true;
                btnSelectFile.Enabled = true;
            }
        }

        private void InitializeEventHandlers()
        {
            _eventBinder.BindCtrl(
                h => btnSelectFile.Click += h,
                h => btnSelectFile.Click -= h,
                (_, __) =>
                {
                    using (var ofd = new OpenFileDialog { Filter = _config.FileFilter })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txtResultPath.Text = ofd.FileName;
                        }
                    }
                });

            _eventBinder.BindCtrl(
                h => btnApply.Click += h,
                h => btnApply.Click -= h,
                (s, e) =>
                {
                    if (!ValidateAndCollectInputs()) return;

                    DialogResult = DialogResult.OK;
                    Close();
                });
        }

        private bool ValidateAndCollectInputs()
        {
            if (_config.HasOffset)
            {
                if (string.IsNullOrWhiteSpace(txtResultOffset.Text))
                {
                    ShowWarning("アドレスを入力してください。");
                    return false;
                }
                Result.Offset = txtResultOffset.Text.ParseStringToInt();
            }

            if (_config.HasCombo)
            {
                if (cmbResultIndex.SelectedIndex == Constants.InvalidValue)
                {
                    ShowWarning("インデックスを選択してください。");
                    return false;
                }
                Result.Index = cmbResultIndex.SelectedIndex;
            }

            if (_config.HasNumeric)
            {
                var value = (int)nudResultCount.Value;
                if (value < nudResultCount.Minimum || value > nudResultCount.Maximum)
                {
                    ShowWarning("有効な値を入力してください。");
                    return false;
                }
                Result.Count = value;
            }

            if (_config.HasFile)
            {
                string path = txtResultPath.Text;
                if (string.IsNullOrEmpty(path))
                {
                    ShowWarning("ファイルを選択してください。");
                    return false;
                }
                Result.Path = path;
            }

            return true;

            // 警告文表示メソッド
            void ShowWarning(string message)
            {
                MessageBox.Show(
                    message, 
                    "", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
            }
        }
    }
}
