using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;

namespace UserControl_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



        }

        public List<UserControl1> userControlList { get; set; } = null!;
        //Nullのダミー　書き手がNullではないことを示す


        IniManagedData IniData { get; set; } = new IniManagedData();



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            userControlList = new List<UserControl1>();
            var userControlName = IniManagedData.ControlField.OriginalUserControl;
            //エイリアスする

            for (int Count = 0; Count < 10; Count++)
            {
                Thickness marthick = new Thickness(10, 10, 0, 0);

                var useC = new UserControl1()
                {
                    Margin = marthick,
                    FontSize = 14,
                    Width = 480,

                    Name = userControlName + $"{Count}"
                    //名前と番号を振っておく
                };




                if (!mainPanel.Children.Contains(useC))
                    mainPanel.Children.Add(useC);


                userControlList.Add(useC);

                //HashSetなので重複チェックは省く


                foreach (var userc in userControlList)
                {
                    userc.Loaded += OriginalUserControl_Load;

                }

            }
        }


        private bool firstSet { get; set; }
        public void OriginalUserControl_Load(object sender, RoutedEventArgs e)
        {
            /////初回のみ呼ばれるようにする
            ///これはタブ切り替えの際に再度Loadされてしまうのでその対処
            firstSet = userControl_setProperties(firstSet);
        }

        private bool userControl_setProperties(bool _firstSet)
        {
            int i = 0;
            if (!_firstSet)
            {


                foreach (var childControl in userControlList)
                {
                    childControl.ArgumentEditor.Text = IniDefinition.GetValueOrDefault
                        (IniData.iniPath, IniManagedData.ControlField.OriginalUserControl + "_" + $"{i}", IniSettingsConst.Arguments_ + $"{i}",
                        "");

                   
                    //selector.ArgumentEditor.Text);

                    childControl.ParamLabel.Text = IniDefinition.GetValueOrDefault(IniData.iniPath, IniManagedData.ControlField.OriginalUserControl + "_" + $"{i}",
                    IniSettingsConst.ParameterLabel + "_" + $"{i}",
                 "パラメータ名").Replace("\r\n","" , true ,CultureInfo.CurrentCulture);

                    string userCCount; //ユーザーコントロールの数

                    userCCount = IniDefinition.GetValueOrDefault(IniData.iniPath, "CheckState", IniManagedData.ControlField.OriginalUserControl + "_Check", "0");
                    int rcountInt = int.Parse(userCCount, CultureInfo.CurrentCulture);
                    //int.Parseでstring型をint型に変換




                    i++;

                    if (childControl.Name == IniManagedData.ControlField.OriginalUserControl + userCCount)
                    {
                        childControl.SlectorRadio.IsChecked = true;
                        IniData.usedOriginalArgument = childControl.ArgumentEditor.Text;
                    }
                }

                return _firstSet;
            }
            else
            {
                _firstSet = false;
            }

            return _firstSet;

        }


        private void LoaddIniButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(userControlList.Count);
        }

        private void WriteIniButton_Click(object sender, RoutedEventArgs e)
        {
            ParamSave_Procedure();
            MessageBox.Show("iniに書き込みました");
        }


        public void ParamSave_Procedure()
        {

            var controlName = IniManagedData.ControlField.OriginalUserControl;
            int i = 0;

            //Add Number and Save setting.ini evey selector 
            foreach (var childCon in userControlList)
            {

                IniDefinition.SetValue(IniData.iniPath, controlName + "_" + $"{i}", "Arguments_" + $"{i}",
                    childCon.ArgumentEditor.Text);

                IniDefinition.SetValue(IniData.iniPath, controlName + "_" + $"{i}", IniSettingsConst.ParameterLabel + "_" + $"{i}",
                    childCon.ParamLabel.Text);

                i++;



                //if Check Selector Radio, Save Check State
                if (childCon.SlectorRadio.IsChecked is not null)
                    if (childCon.SlectorRadio.IsChecked.Value)
                    {
                        var radioCount = childCon.Name.Remove(0, controlName.Length);
                        IniDefinition.SetValue(IniData.iniPath, "CheckState", controlName + "_Check", radioCount);

                    }
            }
        }
    }
}