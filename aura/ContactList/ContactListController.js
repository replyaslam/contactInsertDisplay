({
    doInit : function(component, event, helper) {
        helper.getContacts(component);
    },
	
    handleAddToContactList : function(component, event, helper) {
        helper.addToContactList(component,event);
    },
    
    //handle page previous event
    handlePagePrevious : function(component, event, helper) {
        var currentPageNumber=component.get("v.currentPageNumber") || 1;
        currentPageNumber=currentPageNumber-1;
        helper.getContacts(component,currentPageNumber);
    },
    
    //handle page next event
    handlePageNext : function(component, event, helper) {
        var currentPageNumber=component.get("v.currentPageNumber") || 1;
        currentPageNumber=currentPageNumber+1;
        helper.getContacts(component,currentPageNumber);
    },
})