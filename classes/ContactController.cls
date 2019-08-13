public class ContactController {
    
    @AuraEnabled
    public static PageResult getContactsDB(Decimal pageSize,Decimal startPageNumber) {
        
        //1. pagesize
        Integer pSize=(Integer) pageSize;
        
        //2. offset - start of record number
        Integer offset=((Integer) startPageNumber-1) * pSize;

        //3. return currrentPage, contacts for this page, count of contacts
        PageResult res=new PageResult();        
        res.currentPageNumber=(Integer) startPageNumber;        
        res.contactList= [SELECT Id, LastName, MailingStreet, Phone, Email, LeadSource FROM Contact
                          order by LastName
                          limit :pSize OFFSET :offset                          
                         ];
		//res.contactList.size();
        res.totalNumberOfRecords=[SELECT Count() FROM Contact];
        
        return res;
    }
    
    @AuraEnabled
    public static Contact setContactDB(Contact newContactDB) {
        Contact con=new Contact();
        con.LastName=newContactDB.LastName;
        insert con;
        return con;
        //return [select Id, Name,email from contact limit 20];  
    }
    
    public class PageResult
    {
        //@AuraEnabled
        //public Integer pageSize { get;set;}
        
        @AuraEnabled
        public Integer currentPageNumber {get;set;}
        
        @AuraEnabled
        public Integer totalNumberOfRecords {get;set;}
        
        @AuraEnabled
        public List<Contact> contactList {get;set;}
        
    }
}