using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMath
{
    public class BigInt
    {
        public List<int?> Numbers;

        public BigInt()
        {
            Numbers = new List<int?>();
        }

        public BigInt(string number)
        {
            Numbers = new List<int?>();
            var b = Parse(number);
            this.Numbers = b.Numbers;
        }

        public BigInt(int num)
        {
            Numbers = new List<int?>();
            var b = Parse(num.ToString());
            this.Numbers = b.Numbers;
        }

        public static BigInt Parse(string number)
        {
            BigInt finalNumber = new BigInt();
            char[] numberArray = number.ToCharArray();

            for (int i = numberArray.Length - 1; i >= 0; i--)
            {
                int? parsedInt = int.Parse(numberArray[i].ToString());
                finalNumber.Numbers.Add(parsedInt);
            }
            return finalNumber;
        }

        public static BigInt operator +(BigInt n1, BigInt n2)
        {
            BigInt ans = new BigInt();
            if (n1.Numbers.Count < n2.Numbers.Count)
            {
                int difference = n2.Numbers.Count - n1.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n1.Numbers.Add(0);
                }
            }
            if (n2.Numbers.Count < n1.Numbers.Count)
            {
                int difference = n1.Numbers.Count - n2.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n2.Numbers.Add(0);
                }
            }

            int carry = 0;

            for (int i = 0; i < n1.Numbers.Count; i++)
            {
                int? num = 0;
                if (carry > 0)
                {
                    num += carry;
                    carry = 0;
                }

                num += n1.Numbers[i] + n2.Numbers[i];

                if (num > 9)
                {
                    num -= 10;
                    carry = 1;
                }

                ans.Numbers.Add(num);
            }

            if (carry > 0)
                ans.Numbers.Add(carry);

            return ans;
        }

        public static BigInt operator -(BigInt n1, BigInt n2)
        {
            if (n1.Numbers.Count < n2.Numbers.Count)
            {
                int difference = n2.Numbers.Count - n1.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n1.Numbers.Add(0);
                }
            }
            if (n2.Numbers.Count < n1.Numbers.Count)
            {
                int difference = n1.Numbers.Count - n2.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n2.Numbers.Add(0);
                }
            }
            BigInt ansBigInt = Complement(n2);
            ansBigInt += n1;
            ansBigInt.Numbers.RemoveAt(ansBigInt.Numbers.Count - 1);
            return ansBigInt;
        }

        public static BigInt operator *(BigInt n1, BigInt n2)
        {
            BigInt ans = new BigInt();

            for (int mulIterative = 0; mulIterative < n2.Numbers.Count; mulIterative++)
            {
                BigInt mul = new BigInt();
                int carry = 0;

                for (int i = 0; i < mulIterative; i++)
                {
                    mul.Numbers.Add(0);
                }

                for (int i = 0; i < n1.Numbers.Count; i++)
                {
                    int? a = 0;
                    if (carry > 0)
                    {
                        a += carry;
                        carry = 0;
                    }
                    a += n1.Numbers[i] * n2.Numbers[mulIterative];
                    carry = (int)Math.Floor((double)a / 10d);
                    mul.Numbers.Add(a % 10);
                }

                a:
                if (carry > 0)
                {
                    if (carry < 10)
                    {
                        mul.Numbers.Add(carry);
                    }
                    else
                    {
                        mul.Numbers.Add(carry % 10);
                        carry = (int)Math.Floor((double)carry / 10d);
                        goto a;
                    }
                }

                ans += mul;
            }
            return ans;
        }

        public static BigInt operator /(BigInt n1, BigInt n2)
        {
            BigInt ans = new BigInt();
            BigInt carry = new BigInt();
            for (int i = n1.Numbers.Count - 1; i >= 0; i--)
            {
                int? currentNum = n1.Numbers[i];
                BigInt reverseHolder = new BigInt();
                reverseHolder.Numbers.Add(currentNum);
                for (int j = 0; j < carry.Numbers.Count; j++)
                {
                    reverseHolder.Numbers.Add(carry.Numbers[j]);
                }
                ans.Numbers.Add(MiniDivide(reverseHolder,n2,out carry));
            }
            BigInt holder = new BigInt();
            for (int i = ans.Numbers.Count - 1; i >= 0; i--)
            {
                holder.Numbers.Add(ans.Numbers[i]);
            }
            return holder;
        }

        public static BigInt operator %(BigInt n1, BigInt n2)
        {
            BigInt ans = new BigInt();
            ans = n1/n2;
            ans *= n2;
            ans = n1 - ans;
            return ans;
        }

        private static int? MiniDivide(BigInt num, BigInt divisor, out BigInt carry)
        {
            int? ans = 0;
            while (num >= divisor)
            {
                ans += 1;
                num -= divisor;
            }
            carry = num;
            return ans;
        }

        public static bool operator ==(BigInt n1, BigInt n2)
        {
            if (n1.Numbers.Count < n2.Numbers.Count)
            {
                int difference = n2.Numbers.Count - n1.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n1.Numbers.Add(0);
                }
            }
            if (n2.Numbers.Count < n1.Numbers.Count)
            {
                int difference = n1.Numbers.Count - n2.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n2.Numbers.Add(0);
                }
            }

            for (int i = 0; i < n1.Numbers.Count; i++)
            {
                if (n1.Numbers[i] != n2.Numbers[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(BigInt n1, BigInt n2)
        {
            if (n1 == n2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool operator <(BigInt n1, BigInt n2)
        {
            if (n1.Numbers.Count < n2.Numbers.Count)
            {
                int difference = n2.Numbers.Count - n1.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n1.Numbers.Add(0);
                }
            }
            if (n2.Numbers.Count < n1.Numbers.Count)
            {
                int difference = n1.Numbers.Count - n2.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n2.Numbers.Add(0);
                }
            }

            for (int i = n1.Numbers.Count - 1; i >= 0; i--)
            {
                if (n1.Numbers[i] < n2.Numbers[i])
                {
                    return true;
                }
                else if (n1.Numbers[i] > n2.Numbers[i])
                {
                    return false;
                }
            }

            return false;
        }

        public static bool operator >(BigInt n1, BigInt n2)
        {
            if (n1.Numbers.Count < n2.Numbers.Count)
            {
                int difference = n2.Numbers.Count - n1.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n1.Numbers.Add(0);
                }
            }
            if (n2.Numbers.Count < n1.Numbers.Count)
            {
                int difference = n1.Numbers.Count - n2.Numbers.Count;
                for (int i = 0; i < difference; i++)
                {
                    n2.Numbers.Add(0);
                }
            }

            for (int i = n1.Numbers.Count - 1; i >= 0; i--)
            {
                if (n1.Numbers[i] > n2.Numbers[i])
                {
                    return true;
                }
                else if(n1.Numbers[i] < n2.Numbers[i])
                {
                    return false;
                }
            }
            return false;
        }

        public static bool operator <=(BigInt n1, BigInt n2)
        {
            if (n1 > n2)
            {
                return false;
            }
            return true;
        }

        public static bool operator >=(BigInt n1, BigInt n2)
        {
            if (n1 < n2)
            {
                return false;
            }
            return true;
        }

        private static BigInt Complement(BigInt number)
        {
            BigInt complement = new BigInt();
            foreach (var num in number.Numbers)
            {
                complement.Numbers.Add(9 - num);
            }
            complement += new BigInt(1.ToString());
            return complement;
        }

        public override string ToString()
        {
            string outString = "";
            var hitFirstNumber = false;

            for (int i = Numbers.Count - 1; i >= 0; i--)
            {
                int? a = Numbers[i];
                if (a != 0)
                {
                    if (!hitFirstNumber)
                    {
                        hitFirstNumber = true;
                    }
                    outString += a.ToString();
                }
                else if (hitFirstNumber == true)
                {
                    outString += a.ToString();
                }
                if (i == 0 && Numbers[i] == 0 && !hitFirstNumber)
                {
                    outString += "0";
                }
            }

            return outString;
        }
    }
}
