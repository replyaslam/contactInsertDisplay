//AI20190118 -  Integration with Marketinc cloud (Triggered Send)
//		Author: Aslam Iqbal
//		
//		
public class MarketingCloudIntegration 
{    
    @future(callout=true)
    public static void sendmail(string customerKey,string ToEmail,string SubscriberKey,string LoyaltyID)
    {   
        //1. get clientid,clientSecret, endpoint urls from the Custom metadata
        string authentication_Endpoint='';
        string authentication_Sendmail='';
        string clientid;
        string clientSecret;
        List<Integration_Authentication__mdt> lstCustomMetadata=([SELECT clientid__c,clientSecret__c,label, Authentication_Endpoint__c,	Authentication_Sendmail__c,DeveloperName 
                                                                  FROM Integration_Authentication__mdt 
                                                                  where DeveloperName='Marketing_cloud_login_keys']);  
        if (lstCustomMetadata.size()>0)
        {
            clientId=lstCustomMetadata[0].clientid__c;
            clientSecret=lstCustomMetadata[0].clientSecret__c;   
            authentication_Endpoint=lstCustomMetadata[0].Authentication_Endpoint__c;
            authentication_Sendmail=lstCustomMetadata[0].Authentication_Sendmail__c;               
        }    
        
        //2. get access token
        string token=getAccessToken(clientId,clientSecret,authentication_Endpoint);        
        System.debug('token:'+token);
        
        //3. setup Sendmail endPointurl
        //below both endpoint url's work
        //authentication_Sendmail='https://<domainname>.rest.marketingcloudapis.com/messaging/v1/messageDefinitionSends'; //works
        //authentication_Sendmail='https://abcded-ghijk.rest.marketingcloudapis.com/messaging/v1/messageDefinitionSends'; //works - UAT
        //authentication_Sendmail='https://www.exacttargetapis.com/messaging/v1/messageDefinitionSends';//works
        //
        String endPointurl=authentication_Sendmail+'/key:'+customerKey+'/send';
        
        //4. form json request body
        JsonWrapper wrap = new jsonWrapper();
        String requestBody=wrap.getEmailJson(token,ToEmail,SubscriberKey,LoyaltyID);
        
        //5. set the request object
        HttpRequest req = new HttpRequest();
        req.setMethod('POST');
        req.setHeader('Content-Type', 'application/json');
        req.setEndpoint(endPointurl); 
        req.setCompressed(true);      
        req.setHeader('Authorization','Bearer ' + token ); //Pass the access token in HTTP header Authorization parameter.         
        req.setBody(requestBody);
        
        //6. send the http request and capture the response
        Http http = new Http();
        HTTPResponse res = http.send(req);
        
        //7. check the response status code -handle eror
        if (res.getStatusCode() < 200 || res.getStatusCode() >= 300) {
            System.debug(LoggingLevel.WARN, 'Message definition send API could not forward data to Marketing Cloud: ' + res.toString());
            //throw new ServerSideException('Forwarding order data to marketing cloud failed with status code '+ res.getStatusCode() + ' and status ' +res.getStatus());
        }
        else
        {
            System.debug('success:'+ res.getBody());
        }
    }
    
    
    //get marketing cloud access token - this is required to do further calls
    public static string getAccessToken(string clientid,string clientSecret,string authentication_Endpoint)
    {   
        //below both end point work
        //authentication_Endpoint='https://<domainname>.auth.marketingcloudapis.com/v1/requestToken';// works
        //authentication_Endpoint='https://abcdef-hijklm.auth.marketingcloudapis.com/v1/requestToken';// works
        //authentication_Endpoint='https://auth.exacttargetapis.com/v1/requestToken'; //works
        
        //assign clientid,clientSecret to a json wrapper class and build a json body
        JsonWrapper wrap = new JsonWrapper();
        wrap.clientId=clientid;
        wrap.clientSecret=clientSecret;        
        String jsonBody = json.serialize(wrap);

        //Send a request and capture response to get access token        
        HttpResponse resp;
        HttpRequest req = new HttpRequest();
        req.setEndpoint(authentication_Endpoint);
        req.setMethod('POST');
        req.setHeader('Content-Type','application/json');
        req.setBody(jsonBody);
        Http http_req = new Http();
        resp = http_req.send(req);
        Map<String,Object> mapBody = (Map<String,Object>)JSON.deserializeUntyped(resp.getBody());
        
        // get the Session ID from the response from the call to oauth2/token
        String accessToken = (String)mapBody.get('accessToken');
        return accessToken;
    }    
}