using System;

namespace CodeSnippets{
    
    public class CodeSnippets{
        
        /// <summary>
        /// function1 takes string as input
        /// It then coverts the string into an array of character. With each index representing one character
        /// The characters in the array are then reversed i.e parts[0] become parts[len(parts)-1]
        /// It then converts all charcters into a lower cases and stores this in the variable "strap"
        /// var b checks if the lower case is equal to "starp" (the reverse lower case of the input string)
        /// </summary>
        static bool function1(string pattern) {
            var parts = pattern.ToCharArray();
            Array.Reverse(parts);
            var starp = (new string(parts)).ToLower();
            
            var b = pattern.ToLower().Equals(starp);
            return b;
        }
        
        
        /// <summary>
        /// function2 takes an array of integers as input.
        /// The outer loop creates a gap for comparsion: The gap is initialized as half the length of the input array
        /// The gap is divided by 2 in each interation until it becomes 0
        /// The inner loop iterates through each value from starting from the index h till the last value in the array
        /// For each iteration in the inner loop the current value is stored in the variable "temp"
        /// The value of stored in temp is used to comapare to the value from "h" steps to the left of temp
        /// If this value is greater than the value in temp. It shifts it to the right
        /// When the shift of elements greater than the value in temp is done. The value in temp is placed at its correct position "t"
        /// The process is repeated until the input array is arranged in ascending order
        /// The function returns zero when completed
        /// </summary>
        public static int function2(int[] numbers){
            for (var h = numbers.Length / 2; h > 0; h /= 2){
                for (var i = h; i < numbers.Length; i += 1){
                    var temp = numbers[i];
                    int t;
                    for (t = i; t >= h && numbers[t - h] > temp; t -= h){
                        numbers[t] = numbers[t - h];
                    }
                    numbers[t] = temp;
                }
            }
            return 0;
        }
    }
}