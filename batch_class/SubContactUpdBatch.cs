/*-----------------------------------
Created on : 
Description : Update contact-verifieddate with oldest Subscription-VerifiedDate
//SubContactUpdBatch af = new SubContactUpdBatch();
//Database.executeBatch(af,200);
-----------------------------------*/

global class SubContactUpdBatch implements Database.batchable<sObject>, Database.Stateful{
    global List<String> exceptionList;
    global List<String> activeSubscriptionStatusList = new List<String>();	//exclude delete and unsubscribed subscription
    global Date lastModifiedFromDate;
    
    
    global SubContactUpdBatch(){
        exceptionList = new List<String>();//initialize exceptionList
        
        lastModifiedFromDate=System.today().adddays(-1);
        //Active subscriptions - these subscriptions are consideed active
        activeSubscriptionStatusList.add('Active');
        activeSubscriptionStatusList.add('Verified');
        activeSubscriptionStatusList.add('Lapsing');
        activeSubscriptionStatusList.add('Lapsed');
        activeSubscriptionStatusList.add('Non-engaged');
    }
    
    //starthttps://uat--maguat.cs101.my.salesforce.com/_ui/common/apex/debug/ApexCSIPage#
    global Database.QueryLocator start(Database.BatchableContext BC){
        string sQuery='';
		//squery='SELECT Id ,Status__c, (SELECT Id, Status__c FROM Subscriptions__r), (SELECT id FROM cases) from contact';
        sQuery=sQuery+'select id,Contact__c,LastModifiedDate from Subscription__c';
        sQuery=sQuery+' where VerifiedDate__c!=null';// and LastModifiedDate >=:lastModifiedFromDate';// and Status__c!='+'\''+'Deleted'+'\'';
        sQuery=sQuery+' and Status__c in :activeSubscriptionStatusList';//only active subscriptions
        //sQuery  = sQuery  + ' And CreatedBy.name=\'Aslam Iqbal\''; 
        System.debug('sQuery:'+sQuery);
        //System.debug('lastModifiedFromDate:'+lastModifiedFromDate);
        
        return Database.getQueryLocator(sQuery);
    }
    
    //execute
    global void execute(Database.BatchableContext BC, List<sObject> scope){       
        Set<ID> setContactIDs=new Set<ID>();
        try
        {
            //1. get all ContactIDs for scoped subscriptions
            for (Subscription__c sub:(List<Subscription__c>)scope)
            {   
                System.debug('sub:'+sub);
                setContactIDs.add(sub.Contact__c);
            }
            
            //2. get all subscriptions for the set of contacts - into map-of-contact_And_ListOfSubscriptions 
            map<ID,List<Subscription__c>> mapContactAndSubscriptionsList=new map<ID,List<Subscription__c>>();
            for(Subscription__c sub:([SELECT id,contact__c,VerifiedDate__c,contact__r.VerifiedDate__c from Subscription__c
                                          where contact__c in :setContactIDs
                                          and status__c in :activeSubscriptionStatusList//only active subscriptions
                                         ]))
            {
                if(!mapContactAndSubscriptionsList.containsKey(sub.contact__c))
                {
                    mapContactAndSubscriptionsList.put(sub.contact__c, new List<Subscription__c>()); //We need to initialise the list
                }
                mapContactAndSubscriptionsList.get(sub.contact__c).add(sub);
            }
            
            //3. get the oldest subscription verified-date into a map 
            Set<ID> setContactToUpdate=new set<ID>();
            map<ID,Date> mapContactVerifiedDate=new map<ID,Date>();
            for (ID ContactID:mapContactAndSubscriptionsList.keySet())
            {
                Date verifiedDate=null;
                for(Subscription__c sub: mapContactAndSubscriptionsList.get(ContactID))
                {    
                    //set variable 'verified date' with the oldest subscription->verifiedDate
                    if (verifiedDate==null || verifiedDate>sub.VerifiedDate__c)
                        verifiedDate=sub.VerifiedDate__c;
                }
                mapContactVerifiedDate.put(ContactID,verifiedDate);//map of contactID and oldest VerifiedDate
                setContactToUpdate.add(ContactID);
            }
            
            //4.get contacts where we need to update the verifeid date
            List<contact> lstContact=new List<Contact>();
            for(Contact con:[Select ID,VerifiedDate__c from contact where id in:setContactToUpdate])
            {
                if (con.VerifiedDate__c!=mapContactVerifiedDate.get(con.ID))
                {
                    con.VerifiedDate__c=mapContactVerifiedDate.get(con.ID);
                    lstContact.add(con);
                }
            }
            
            //5.now update the contacts with new verified date
            if (lstContact.size()>0)
                update lstContact;
            
        }
        Catch(Exception e){
            exceptionList.add('Error: '+e.getMessage()+' => LineNumber: '+e.getLineNumber()+' => Stack trace: '+e.getStackTraceString());        
        }
    }
    
    
    //method to send success /failed email
    global void finish(Database.BatchableContext BC){
        
        //Send email (if not called from test class)
        if(!Test.isRunningTest())
        {
            Utility.sendBatchCompletionEmail('Contact VerifiedDate update batch process completed',exceptionList);
        }
    }    
    
}