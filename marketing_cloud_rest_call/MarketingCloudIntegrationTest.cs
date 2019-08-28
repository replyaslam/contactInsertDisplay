//AI20190402 - test class for MarketingCloudIntegration
//		Author: Aslam Iqbal
//		

@isTest()
public class MarketingCloudIntegrationTest {
    
     @isTest static void testSendmail() 
    {           
        string customerKey = 'Account_Closed';
        string ToEmail='askaslam@gmail.com';
        string SubscriberKey='0030N00002Td9SWQAZ';        
        string LoyaltyID='0030N00002Td9SWQAZ';
        // This causes a fake response to be generated
        Test.setMock(HttpCalloutMock.class, new SendmailCalloutMock());
        
        // Call the method that invokes a callout
        MarketingCloudIntegration.sendmail(customerKey,ToEmail,SubscriberKey,LoyaltyID);
        
        // Verify that a fake result is returned
        //System.assertEquals('Mock response', output); 
    } 

}