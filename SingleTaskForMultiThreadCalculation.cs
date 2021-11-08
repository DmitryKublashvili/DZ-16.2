using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DZ_16._2
{
    class SingleTaskForMultiThreadCalculation
    {
        public int Result { get; private set; }

        public void Calculate(int startPoint, int endPoint)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            // последние цифры чисел можно не суммировать, так как если сумма предыдущих будет кратна последней цифре
            // то и данная сумма, увеличенная на эту цифру всегда буде кратная данной цифре 
            int startNum = startPoint / 10;
            int finishNum = endPoint / 10;

            // определяем сумму цифр исходного числа
            var sum = startNum.ToString().                        // переводим в текст
                Select(simbol => int.Parse(simbol.ToString())).   // формируем перечисление цифр
                Aggregate((x, y) => x + y);                       // применяем агрегатную функцию

            // последняя цифра стартового числа
            int digit = startNum % 10;

            // количество десятков в стартовом числе
            int tens = startNum / 10;

            // переменная, в которую будет собираться рузультат вычислений
            int result = 0;

            for (int i = startNum; i < finishNum; i++)
            {
                digit++;

                // сумма цифр числа всегда больше на 1 чем сумма цифр предыдущего, за исключением ситуации, когда увеличивается количество десятков (9 => 0)
                // обработаем данную ситуацию. Увеличение десятков всегда дает уменьшение суммы цифр по отношению к предыдущему числу на (9*n-1)
                // где n - коэффициент, равный разрядности увеличения количества десятков
                if (digit != 10)
                {
                    sum++;
                }
                else
                {
                    tens++;   // количество десятков увеличиаем на 1

                    digit = 0;  // последняя цифра обращается в 0 в данной ситуции

                    int coefficient = 1;  // по умолчанию коэффициент = 1 (то есть подразумевается, что в ноль обратилась только последняя цифра числа

                    // если же в ноль обратились и другие цифры, то сумма цифр по отношению к сумме цифр предыдущего числа должна быть уменьшена
                    // в соответствии с вышеприведенной формулой

                    // определим коэффициент
                    if (tens % 10 != 0)
                    {
                        coefficient = 1;
                    }
                    else
                    {
                        if (tens % 10 == 0) coefficient = 2;
                        if (tens % 100 == 0) coefficient = 3;
                        if (tens % 1000 == 0) coefficient = 4;
                        if (tens % 10_000 == 0) coefficient = 5;
                        if (tens % 100_000 == 0) coefficient = 6;
                        if (tens % 1000_000 == 0) coefficient = 7;
                        if (tens % 10_000_000 == 0) coefficient = 8;
                        if (tens % 100_000_000 == 0) coefficient = 9;
                    }
                    // уменьшаем сумму цифр по отношению к предыдущей
                    sum -= (coefficient * 9 - 1);

                }
                //проверяем суммы цифр на делимость
                ++result;   // деление на 1
                if (sum % 2 == 0) ++result;
                if (sum % 3 == 0) ++result;
                if (sum % 4 == 0) ++result;
                if (sum % 5 == 0) ++result;
                if (sum % 6 == 0) ++result;
                if (sum % 7 == 0) ++result;
                if (sum % 8 == 0) ++result;
                if (sum % 9 == 0) ++result;
            }
            //stopwatch.Stop();

            Result = result;
        }
    }
}
