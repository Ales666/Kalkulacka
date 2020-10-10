using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kalkulacka
{
    public partial class MainPage : ContentPage
    {
        //současný stav, podle kterého se bude rozhodovat o přiřazení čísla buďto do firstNumber nebo secondNumber
        int currentState = 1;
        string operace;
        //do firstNumber a secondNumber se budou ukládat čísla pro výpočet
        double firstNumber, secondNumber;

        public MainPage()
        {
            InitializeComponent();
            OnClear(this, null);
        }

        void OnNumber(object sender, EventArgs e)
        {
            //ověření, než bude stisknuté tlačítko
            Button button = (Button)sender;

            string pressed = button.Text;

            //pokud je výsledek 0, přesměruje kalkulačku, aby vyloučila 0
            //při provním "projetí" je stav 1
            if (this.resultText.Text == "0" || currentState < 0)
            {
                //textová hodnota bude po stisknutí tlačítka vymazána
                this.resultText.Text = "";

                //při prvním projetí je hodnota 1. Toto funguje pokud dále pracujeme již s výsledkem
                if (currentState < 0)
                    currentState *= -1;
            }

            this.resultText.Text += pressed; 

            double number;
            if (double.TryParse(this.resultText.Text, out number))
            {
                this.resultText.Text = number.ToString("N0");
                if (currentState == 1)
                {
                    //při prvním projetí bude hodnota 1, takže se hodnota čísla zapíše do firstNumber
                    firstNumber = number;
                }
                else
                {
                    //při druhém projetí nebo další práci s výsledkem se hodnota čísla zapíše do secondNumber
                    secondNumber = number;
                }
            }
        }

        //volá se, když se zadá "číslicový operátor" (+, -, *, /)
        void OnOperator(object sender, EventArgs e)
        {
            currentState = -2;
            Button button = (Button)sender;
            string pressed = button.Text;
            operace = pressed;
        }

        //volá se, když se zmáčkne C (reset)
        void OnClear(object sender, EventArgs e)
        {
            firstNumber = 0;
            secondNumber = 0;
            currentState = 1;
            this.resultText.Text = "0";
        }

        //volá se, když je vyplněné jak firstNumber, tak secondNumber a uživatel zmáčkne =
        void OnCalculate(object sender, EventArgs e)
        {
            if (currentState == 2)
            {
                var result = Calculate(firstNumber, secondNumber, operace);

                this.resultText.Text = result.ToString();
                firstNumber = result;
                currentState = -1;
            }
        }
       
        public static double Calculate(double value1, double value2, string myoperator)
        {
            double result = 0;
            //podle typu "číslicového operátora" se provede výpočet
            switch (myoperator)
            {
                case "÷":
                    result = value1 / value2;
                    break;
                case "*":
                    result = value1 * value2;
                    break;
                case "+":
                    result = value1 + value2;
                    break;
                case "-":
                    result = value1 - value2;
                    break;

            }
            return result;

        }

        //volá se, když je vyplněné pouze firstNumber a zmáčkne se tlačítko pro odmocnění
        void Odmocnina(object sender, EventArgs e)
        {
            if ((currentState == -1) || (currentState == 1))
            {

                var result = Math.Sqrt(firstNumber);

                this.resultText.Text = result.ToString();
                firstNumber = result;
                currentState = -1;
            }
        }

        //volá se, když je vyplněné pouze secondNumber a zmáčkne se tlačítko pro umocnění
        private void Mocnina(object sender, EventArgs e)
        {

            if ((currentState == -1) || (currentState == 1))
            {

                var result = firstNumber * firstNumber;
                this.resultText.Text = result.ToString();
                firstNumber = result;
                currentState = -1;
            }
        }

    }
}