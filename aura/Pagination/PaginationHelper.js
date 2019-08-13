({
    previousEventFire : function(component,event) {
        var cmpEvent = component.getEvent("pagePrevious");
        //cmpEvent.setParams({"direction":"next" });
        cmpEvent.fire();               
    },
    
    nextEventFire : function(component,event) {
        var cmpEvent = component.getEvent("pageNext");
        //cmpEvent.setParams({"direction":"next" });
        cmpEvent.fire();              
    },    
})