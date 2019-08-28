/*------------------------------------------
    Desc : test class for SubContactUpdBatchSchedule class
    Aslam Iqbal
--------------------------------------------*/

@isTest
public class SubContactUpdBatchScheduleTest {
        
    static testmethod void schedulerTest() 
    {
        String CRON_EXP = '0 0 23 ? * *';        
          
        Test.startTest();        
        String jobId = System.schedule('SubContactUpdBatchScheduleTest',  CRON_EXP, new SubscriptionContactUpdateBatchSchedule());
        CronTrigger ct = [SELECT Id, CronExpression, TimesTriggered, NextFireTime FROM CronTrigger WHERE id = :jobId];
        System.assertEquals(CRON_EXP, ct.CronExpression);
        System.assertEquals(0, ct.TimesTriggered);        
        Test.stopTest();
      
    }    
}