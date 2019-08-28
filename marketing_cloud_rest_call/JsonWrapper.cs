//AI20190118 -  Integration with Marketinc cloud (Triggered Send)
//Description:utility apex class for making Request Body, that returns the request body JSON for messageDefinitionSends API POST callout
//Author: Aslam Iqbal

public class JsonWrapper 
{
    
    //security keys - get from custom metadata
    public String clientId {get; set;}
    public String clientSecret {get; set;} 
    
    //get formated email json
    public string getEmailJson(string token,string ToEmail,string SubscriberKey,string LoyaltyID)
    {
        string data='';
        data=data+'{';        
        data=data+'"To": {';
        data=data+'    "Address": "'+ ToEmail + '",';
        data=data+'    "SubscriberKey": "'+SubscriberKey+'",';
        data=data+'    "ContactAttributes": {';
        data=data+'        "SubscriberAttributes": {';
		data=data+'           "LoyaltyID": "'+LoyaltyID+'"';                 
        data=data+'                }';
        data=data+'    }';        
        data=data+'},';
        data=data+'"Options": {';
        data=data+'    "RequestType": "SYNC"';
        data=data+' }';
        data=data+'}';
        
        return data;
    }    
}