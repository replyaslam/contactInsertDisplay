@isTest
global class SendmailCalloutMock implements HttpCalloutMock{
    global HttpResponse respond(HTTPRequest req){
        HttpResponse res = new HttpResponse();
        res.setStatus('OK');
        res.setStatusCode(200);
        //res.setBody('GREAT SCOTT');
        string s='{"requestId":"d28f8f6c-6a61-46a6-87a3-1988c29b1da1","responses":[{"recipientSendId":"d28f8f6c-6a61-46a6-87a3-1988c29b1da1","hasErrors":false,"messages":["Queued"]}]}';
        res.setBody(s);
        return res;
    }
}