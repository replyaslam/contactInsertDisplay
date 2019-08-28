({
    getContacts : function(component,  startPageNumber) {

        //1. get reference to serverside controller action/method
        var action=component.get('c.getContactsDB');
        var pageSize=component.get('v.pageSize');

        //2. get clientside values and set server side controller parameters
        //var contList=comp.get("v.contactList");
		
        //3. set parameters - pagesize and start page number (if not specified then 1)
        action.setParams({"pageSize":pageSize,
                          "startPageNumber":startPageNumber ||1
                         });
        
        //4. set callback
        action.setCallback(true, function(response){
            var state=response.getState();            
            if (state==="SUCCESS" && component.isValid())
            {
                var result=response.getReturnValue();
                component.set("v.contactList",result.contactList);
                component.set("v.currentPageNumber",result.currentPageNumber);
                component.set("v.totalNumberOfPages",Math.ceil(result.totalNumberOfRecords/pageSize));
                component.set("v.totalNumberOfRecords",result.totalNumberOfRecords);                
                console.log('success');
            }else{
                var errors=response.getError();
                if(errors){
                    if(errors[0] && errors[0].message){
                        console.log('error');
                        component.set("v.errorMessage",errors[0].message);
                        component.set("v.isError",true);
                    }
                }else{
                    component.set("v.errorMessage","unown error, response state:" + response.getState());
                    component.set("v.isError",true);
                }
            }            
        });

        //5. fire event        
        $A.enqueueAction(action);
    },
    
     addToContactList : function(comp,evt) {
         var contNew=evt.getParam('cont');
         //console.log('contNewHelper:'+JSON.stringify(contNew, null, 4));
         var contList=comp.get("v.contactList");
         console.log('contListHelper:'+JSON.stringify(contList, null, 4));
         
         contList.unshift(contNew);
         console.log('contListHelper:'+ JSON.stringify(contList, null, 4));

         comp.set("v.contactList",contList);
    } 
    
    
})