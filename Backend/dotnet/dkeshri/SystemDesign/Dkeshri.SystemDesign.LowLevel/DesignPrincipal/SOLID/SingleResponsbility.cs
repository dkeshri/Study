namespace Dkeshri.SystemDesign.LowLevel.DesignPrincipal.SOLID

{
    
    
    // Single responsibility principal.



    public class ApiHandlerWrong{
        public object fetchData(string url){

            // Logic to hit the api http Request.
            return new object();
        }
        public object ParseData(){
            // Login to Parse the return object from fetchData.
            return new object();
        }
        public bool SaveDataToDataBase(){
            // save parse dara to database.

            return true;
        }
    }


    // correct way to implement Single Responsibility.
    public class ApiHandler{
        // this class has only one responsibiliy to calling the Object method.
        FetchData fetchData;
        ParseData parseData;
        SaveData saveData;

        public ApiHandler(){
            fetchData = new FetchData();
            parseData = new ParseData();
            saveData = new SaveData();
        }
        
        public void handleModules(){
            // fetch the data from URL
            var Data =  fetchData.fetchData("https://test.com/getItems");

            // Parse Data

            var ParsedData = parseData.parseData(Data);

            // Save data to Database.

            var SaveData = saveData.SaveDataToDataBase(ParsedData);

        }
    }
    public class FetchData{
        public object fetchData(string url){
            // Logic to hit the api http Request.
            return new object();
        }
    }
    public class ParseData{
        public object parseData(object obj){
            // Login to Parse the return object from fetchData.
            return new object();
        }
    }
    public class SaveData{
        public bool SaveDataToDataBase(object obj){
            // save parse dara to database.

            return true;
        }
    }
}