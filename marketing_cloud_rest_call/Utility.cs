//AI20190108 - This class will have all the commonly used methods, that can be called from different classes

public class Utility 
{      
    
    public static void CreateDocumentCSV(String FolderName,String FileName,string csvColumnHeader,List<String> csvRowValues)
    {
        List<Folder> folders = [SELECT Id, Name FROM Folder WHERE Name =: FolderName];            
        String csvFileContent = csvColumnHeader + String.join(csvRowValues,'\n');
        if(!folders.isEmpty()){
            Document doc = new Document(Name = FileName, Body = Blob.valueOf(csvFileContent), FolderId = folders[0].Id, Type = 'csv', ContentType='application/vnd.ms-excel');
            insert doc;
        }        
    }
    
    public static void sendBatchCompletionEmail(String subjectLine,List<string> exceptionList)
    {
        //1. Build 'to' email addresses
        List<Email_Custom_Setting__c> emailCustSettingList = Email_Custom_Setting__c.getall().values();        
        List<string> emailList = new List<String>();
        if(!emailCustSettingList.isEmpty()){
            for(Email_Custom_Setting__c ecs : emailCustSettingList){
                emailList.add(ecs.Email_ID__C);
            }
        }
        
        //2. Buld emailbody
        String emailBody;       
        if(!exceptionList.isEmpty()){
            for (String exceptionMsg : exceptionList){
                emailBody += exceptionMsg;
            }
            emailBody=subjectLine+' with below errors. <br/>' + emailBody;
        }
        else
            emailBody=subjectLine+' Successfully';
        
        
        //3. Build email
        String replyToEmail='neil.townsend@mcarthurglen.com';//we should change this to store in custom settings?
        List<Reply_To_email__mdt> lstReplyEmail=([SELECT Reply_To_email__c FROM Reply_To_email__mdt limit 1]); 
        if (lstReplyEmail.size()>0)            
            replyToEmail=lstReplyEmail[0].Reply_To_email__c;        
        System.debug('replyToEmail:'+replyToEmail);
        
        Messaging.SingleEmailMessage mail = new Messaging.SingleEmailMessage();
        mail.setToAddresses(emailList);
        mail.setReplyTo(replyToEmail);
        mail.setSenderDisplayName('McArthurGlen');
        mail.setSubject(subjectLine);
        mail.setPlainTextBody(emailBody); 
        
        Messaging.sendEmail(new Messaging.SingleEmailMessage[] { mail });        
        
    }
    
    
    //convert a Set<String> into a quoted, comma separated String literal for inclusion in a dynamic SOQL Query
    public static string convertSetToCommaDelimitedString(Set<String> mapKeySet)
    {
        String newSetStr = '' ;
        for(String str : mapKeySet)
            newSetStr += '\'' + str + '\',';        
        newSetStr = newSetStr.lastIndexOf(',') > 0 ? '(' + newSetStr.substring(0,newSetStr.lastIndexOf(',')) + ')' : newSetStr ;
        return newSetStr;        
    }    
    
    public static Boolean TriggerSwitchOn()
    {
        Boolean switchOn=true;//default on
        
        //if current user is test user then Off
        List<Test_User__mdt> lstTestUser= [SELECT Login_Email__c FROM Test_User__mdt where Login_Email__c=:UserInfo.getUsername()];
        if (lstTestUser.Size()>0)
            switchOn=false;            
        return switchOn;
    }
    
    
    //used to 
    public static Boolean checkTriggerSwitch(String triggerName)
    {
        //if current user is test user then Off
        List<TriggerSwitch__mdt> mtRec= [SELECT isOn__c FROM TriggerSwitch__mdt where DeveloperName=:triggerName LIMIT 1];
        if (mtRec.Size()>0) return mtRec[0].isOn__c ; 
        else return true;
        
    }    
	
	
	/*
	test class
	
	 
    public  testmethod static void testCreateDocumentCSV()
    {
        String FolderName='Setup Audit Trail Logs';
        String FileName = 'SetupAuditTrailLog_'+  Datetime.now().year() + Datetime.now().Month()+ Datetime.now().Day()+ '_' + Datetime.now().minute()+ Datetime.now().second();
        //string csvColumnHeader = 'LastName, FirstName\n';
      
        List<Contact> lstCon=new List<Contact>();
        List<String> csvRowValues=new List<String>();   
        string csvFieldHeader='';
        Map<String, Object> fieldValues;
        String row='';
        
        Contact con1=new Contact();        
        con1.LastName='testLastName1'; 
        con1.FirstName='testFirstName1'; 
        lstCon.add(con1);
        
        Contact con2=new Contact();
        con2.LastName='testLastName2'; 
        con2.FirstName='testFirstName2'; 
        lstCon.add(con2);        
        insert lstCon;
       
    

        for (Contact con:lstCon)
        {            
            //create header
            if (csvFieldHeader=='')
            {
                fieldValues = con.getPopulatedFieldsAsMap();
                for (String fieldName : fieldValues.keySet()) {
                    csvFieldHeader+=fieldName + ',';
                }
                csvFieldHeader=csvFieldHeader.removeEnd(',')+'\n'; 
            }
            
			
            //get values
            if (fieldValues!=null)
            {
                for (String fieldName : fieldValues.keySet()) {
                    System.debug(fieldName + '=' + con.get(fieldName));
                    row+=con.get(fieldName) +',';                    
                }
                if (row!='')
                    csvRowValues.add(row);
                row='';
            }
            
        }
        
        System.debug('UnityTest-csvFieldHeader:' + csvFieldHeader);
        //System.debug('FolderName:' + FolderName);
        //System.debug('FileName:' + FileName);
        //System.debug('UnityTest-csvColumnHeader:' + csvColumnHeader);
        System.debug('UnityTest-csvRowValues:' + csvRowValues);
        
        //if (!csvRowValues.isEmpty())
        Utility.CreateDocumentCSV(FolderName,FileName,csvFieldHeader, csvRowValues);
    }
	
	public  testmethod static void testsendBatchCompletionEmail()
    {  
        List<String> exceptionList = new List<String>();
        exceptionList.add('error1');
        exceptionList.add('error2');
        exceptionList.add('error3');
  
        List<Email_Custom_Setting__c> ecsList = new List<Email_Custom_Setting__c>();
        Email_Custom_Setting__c ecs1 = new Email_Custom_Setting__c();
        ecs1.Name = 'testMethod1';
        ecs1.Email_ID__C = 'asksalam@gmail.com';
        ecsList.add(ecs1);
        Email_Custom_Setting__c ecs2 = new Email_Custom_Setting__c();
        ecs2.Name = 'testMethod2';
        ecs2.Email_ID__C = 'iqbalsoql@gmail.com';
        ecsList.add(ecs2);
        insert ecsList;
        Utility.sendBatchCompletionEmail('test email',exceptionList);
    }
	*/
    
}