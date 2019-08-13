({
    saveHelper : function(comp) {
        //1. refer server controller method
        var act=comp.get("c.setContactDB");
        
        //2. get the value 
        var cont=comp.get("v.newContact");
        
        //3. set controller parameters (get )
        act.setParams({ newContactDB : comp.get("v.newContact") });
        
        //4. set callback
        act.setCallback(true,function(resp){
            var state=resp.getState();
            //console.log('state:'+resp.getState());
            //console.log('newCont:'+JSON.stringify(resp.getReturnValue(), null, 4));
            
            if(state==="SUCCESS" && comp.isValid())
            {
                var contEvent = $A.get("e.c:AddToContactList");                
                contEvent.setParams({"cont" : resp.getReturnValue()});
                contEvent.fire();
                //console.log('sucess');
            }
        });
        
        //5. fire event
        $A.enqueueAction(act);
    }
})