/* ------------------- DO NOT EDIT ----------------------
 * This file contains custom data-types like enums and structs used in this project
 * You are not required to edit this. */

namespace SimpleShop{
    public enum KeywordTypes{
            String,
            Int,
            Decimal
        } // a custom enumerator type
    
    public struct Keyword{
        // local variables
        private string keyword;
        private KeywordTypes type;

        // constructor
        public Keyword(string keyword, KeywordTypes Type = KeywordTypes.String){
            this.keyword = keyword;
            this.type = Type;
        }

        // custom Struct methods
        public string GetStart(){
            return "<" + this.keyword + ">";
        }

        public string GetEnd(){
            return "</" + this.keyword + ">";
        }

        public string GetString(){
            return this.keyword;
        }

        public KeywordTypes WhichType(){
            return this.type;
        }
    }

    public struct KeywordPair{
        public Keyword Key;
        public string Value;

        public KeywordPair(Keyword key, string value){
            this.Key = key;
            this.Value = value;
        }
    }
}