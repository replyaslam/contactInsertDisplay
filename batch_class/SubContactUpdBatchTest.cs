/*-----------------------------------
Created on : 
Desc : Test class for SubContactUpdBatch
//SubContactUpdBatch af = new SubContactUpdBatch();
//Database.executeBatch(af,200);
-----------------------------------*/


@isTest
public class SubContactUpdBatchTest {
    
    //Test scenarios:
    static testMethod void testSubscriptionContactUpdateBatch()
    {
        //create a test data
        testData td = new testData();
        
        Test.startTest();
        SubContactUpdBatch subhd = new SubContactUpdBatch();
        ID jobID = Database.executeBatch(subhd);        
        Test.StopTest();
        
        //Assert        
        for (Contact con:([Select id,VerifiedDate__c,FirstName from contact]))
        {
            System.debug('con.VerifiedDate__c'+con.VerifiedDate__c);
            if (con.FirstName.contains('VerifiedDate_Aslam_1'))
                System.assertEquals(System.today().adddays(-1), con.VerifiedDate__c);
            else if (con.FirstName.contains('VerifiedDate_Aslam_2'))
                System.assertEquals(System.today().adddays(-30), con.VerifiedDate__c);
        }          
    }
    
    class testData
    {        
        Public testData(){  
            //1. ----------------Create Centre records
            Centre__c centreRec1 = new Centre__c();
            centreRec1.Name = 'Roermond';                   
            Insert centreRec1;
            //Centre__c centreRec1 =[SELECT Id,Name FROM Centre__c WHERE Name = 'Roermond' limit 1];
            
            List<Subscription__c> lstsubscription = new List<Subscription__c>();
            
            //2. ----------------Create Contact records
            integer randm=Integer.valueof((Math.random() * 100));
            Contact contact1 = new Contact();
            contact1.Salutation = 'Ms';
            contact1.FirstName = 'VerifiedDate_Aslam_1_' + Datetime.now().format('yyyyMMddHHmmss');
            contact1.LastName = 'Iqbal';
            contact1.Email = contact1.FirstName+contact1.LastName+'@gmail.com';
            contact1.Country__c = 'United Kingdom';
            contact1.Language__c = 'English';
            contact1.LatestOutlet__c = centreRec1.id;
            contact1.Status__c = 'Not Registered';
            contact1.CreatedDate=System.today().addMonths(-7);
            contact1.LastModifiedDate=System.today().addMonths(-7);
            insert contact1;
            
            //3. create subscriptions
            Subscription__c subscription1 = new Subscription__c();
            subscription1.Name = 'VerifiedDate_ubscription_1_' + Datetime.now().format('yyyyMMddHHmmss');
            subscription1.Centre__c = centreRec1.Name;
            subscription1.Language__c = 'English';
            subscription1.Contact__c = contact1.id; 
            subscription1.EmailOptIn__c = True;
            subscription1.Status__c = 'Active';
            subscription1.VerifiedDate__c=System.today().adddays(-1);
            subscription1.CreatedDate=System.today().adddays(-20);
            subscription1.LastModifiedDate=System.today().adddays(-2);
            lstsubscription.add(subscription1);
            
            Subscription__c subscription2 = new Subscription__c();
            subscription2.Name = 'VerifiedDate_Subscription_2_' + Datetime.now().format('yyyyMMddHHmmss');
            subscription2.Centre__c = centreRec1.Name;
            subscription2.Language__c = 'German';
            subscription2.Contact__c = contact1.id; 
            subscription2.EmailOptIn__c = True;
            subscription2.Status__c = 'Active';
            //subscription2.VerifiedDate__c=System.today().adddays(-2);
            subscription2.CreatedDate=System.today().adddays(-20);
            subscription2.LastModifiedDate=System.today().adddays(-2);
            lstsubscription.add(subscription2);
            
            Subscription__c subscription3 = new Subscription__c();
            subscription3.Name = 'VerifiedDate_Subscription_3_' + Datetime.now().format('yyyyMMddHHmmss');
            subscription3.Centre__c = centreRec1.Name;
            subscription3.Language__c = 'Dutch';
            subscription3.Contact__c = contact1.id; 
            subscription3.EmailOptIn__c = True;
            subscription3.Status__c ='Deleted';
            subscription3.VerifiedDate__c=System.today().adddays(-3);
            subscription3.CreatedDate=System.today().adddays(-20);
            subscription3.LastModifiedDate=System.today().adddays(-3);
            lstsubscription.add(subscription3);
            
            
            //2a. ----------------Create Contact records
            randm=Integer.valueof((Math.random() * 100));
            Contact contact2 = new Contact();
            contact2.Salutation = 'Ms';
            contact2.FirstName = 'VerifiedDate_Aslam_2_' + Datetime.now().format('yyyyMMddHHmmss');
            contact2.LastName = 'Iqbal';
            contact2.Email = contact2.FirstName+contact2.LastName+'@gmail.com';
            contact2.Country__c = 'United Kingdom';
            contact2.Language__c = 'German';
            contact2.LatestOutlet__c = centreRec1.id;
            contact2.Status__c = 'Not Registered';
            contact2.CreatedDate=System.today().addMonths(-7);
            contact2.LastModifiedDate=System.today().addMonths(-7);
            contact2.VerifiedDate__c=System.today().adddays(-1);
            insert contact2;
            
            //3a. create subscriptions
            Subscription__c subscription4 = new Subscription__c();
            subscription4.Name = 'VerifiedDate_Subscription_4_' + Datetime.now().format('yyyyMMddHHmmss');
            subscription4.Centre__c = centreRec1.Name;
            subscription4.Language__c = 'English';
            subscription4.Contact__c = contact2.id; 
            subscription4.EmailOptIn__c = True;
            subscription4.Status__c = 'Active';
            subscription4.VerifiedDate__c=System.today().adddays(-10);
            subscription4.CreatedDate=System.today().adddays(-20);
            subscription4.LastModifiedDate=System.today().adddays(-2);
            lstsubscription.add(subscription4);
            
            Subscription__c subscription5 = new Subscription__c();
            subscription5.Name = 'VerifiedDate_Subscription_5_' + Datetime.now().format('yyyyMMddHHmmss');
            subscription5.Centre__c = centreRec1.Name;
            subscription5.Language__c = 'German';
            subscription5.Contact__c = contact2.id; 
            subscription5.EmailOptIn__c = True;
            subscription5.Status__c = 'Active';
            subscription5.VerifiedDate__c=System.today().adddays(-30);
            subscription5.CreatedDate=System.today().adddays(-20);
            subscription5.LastModifiedDate=System.today().adddays(-2);
            lstsubscription.add(subscription5);
            
            Subscription__c subscription6 = new Subscription__c();
            subscription6.Name = 'VerifiedDate_Subscription_6_' + Datetime.now().format('yyyyMMddHHmmss');
            subscription6.Centre__c = centreRec1.Name;
            subscription6.Language__c = 'Dutch';
            subscription6.Contact__c = contact2.id; 
            subscription6.EmailOptIn__c = True;
            subscription6.Status__c ='Deleted';
            subscription6.VerifiedDate__c=System.today().adddays(-20);
            subscription6.CreatedDate=System.today().adddays(-20);
            subscription6.LastModifiedDate=System.today().adddays(-2);
            lstsubscription.add(subscription6);            
            insert lstsubscription;
        }
    }
}