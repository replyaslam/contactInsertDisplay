({
    //fire pageprevious event
    previousPage : function(cmp, event) {
        var cmpEvent = cmp.getEvent("pagePrevious");
        //cmpEvent.setParams({"direction":"next" });
        cmpEvent.fire();
        
        //helper.previousEventFire(component,event);
    },
    
    //fire pagenext event
    nextPage : function(cmp, event) {
        var cmpEvent = cmp.getEvent("pageNext");
        //cmpEvent.setParams({"direction":"next" });
        cmpEvent.fire();
        
        //helper.nextEventFire(component,event);
    },    
})