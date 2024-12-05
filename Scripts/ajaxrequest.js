//Display Session Timeout information in the Status bar of the window
var sUserInfo;
var iSessionTimeout;
var iSessionTime="";
var sessionflag = true;
function sessionTimeout(){
    if(document.getElementById("sUserInfo")) {
        sUserInfo = document.getElementById("sUserInfo").value;
    }
    if(document.getElementById("iSessionTimeout")){
        iSessionTimeout = document.getElementById("iSessionTimeout").value;
    }
    if(sessionflag && iSessionTimeout != null){
        ShowSessionTimeout();
    }
}
function ShowSessionTimeout()
{
    var iMinutes=0;
    var iSeconds=0;
    if(iSessionTime == "")
        iSessionTime = parseInt(iSessionTimeout/60);
    if (iSessionTimeout > 0)
    {
        iMinutes =  parseInt(iSessionTimeout/60);
        iSeconds =  iSessionTimeout%60;
        if(sUserInfo != "")
            window.status = sUserInfo + ". Your session will expire within " + iMinutes + " minute(s) and " + iSeconds + " second(s).";
        else
            window.status = "Your session will expire within " + iMinutes + " minute(s) and " + iSeconds + " second(s).";

        if(iMinutes == 2 && iSeconds == 0)
        {
            var dtCurr = new Date();
            var iSec = parseInt(dtCurr.getTime()/1000);
            var iMin = iSessionTime - iMinutes;
            alert("Dear User, you have not clicked on any link or button for the last " + iMin + " minutes. Please click on any link or button within next " + iMinutes + " minutes.");
            var dtCurrNew = new Date();
            var iSecNew = parseInt(dtCurrNew.getTime()/1000);
            var iDiff = iSecNew - iSec;
            if(iDiff < iSessionTimeout)
                iSessionTimeout = iSessionTimeout - iDiff;
            else
                iSessionTimeout = 1;
        }
        iSessionTimeout = iSessionTimeout - 1;
        window.setTimeout("ShowSessionTimeout()", 1000);
    }
    else
    {
        window.status = sUserInfo + ". Your session has been expired.";
    }
}

var xmlHttp;
var ajaxImageDisplayFlag = 1;   // This means common ajax image needs to display.. make it zero if you want to hide it
function xmlhttpPost(strActionFlag,paramArray) 
{
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
      strURL = "mainController.do?actionFlag="+strActionFlag;      
      for(i=0;i<paramArray.length;i++)
      {
        strURL = strURL + "&"+ encodeURIComponent(paramArray[i].name) + "=" + encodeURIComponent(paramArray[i].value);
      }
  
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
                processXMLRequest('','');
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.send(null);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}

function GetXmlHttpObject()
{ 
  var objXMLHttp=null;
  if (window.XMLHttpRequest)
  {
    objXMLHttp=new XMLHttpRequest();
  } 
  else if (window.ActiveXObject)
  {
    objXMLHttp=new ActiveXObject("Microsoft.XMLHTTP");
  }
  return objXMLHttp;
}


function readXML(xmldoc){
    xmlObj=xmldoc.documentElement;
    
    var root = xmlObj.getElementsByTagName('param');    
    for (var iNode = 0; iNode < root.length; iNode++) {
      var node = root[iNode];      
      if(node.childNodes(1).firstChild!=null){
        paramName = node.childNodes(0).firstChild.text;
        paramVal = node.childNodes(1).firstChild.text;
        setXML(paramName,paramVal);
      }
    }
  }
  
function setXML(paramName,paramVal)
{
  var elementColl = null;
	var elementObj = null;
	elementColl = document.getElementsByName(paramName);
        
	for(var i = 0; i < elementColl.length; i++) 
  {
		elementObj = elementColl(i);
		if (elementObj) 
    {
        if (elementObj.type=="text" || elementObj.type=="textarea") {
                elementObj.value = paramVal;
                break;
        }
        
				if (elementObj.type=="select-one" ) 
        {
						if(elementObj.options) {
								for (var optionIndex=0; optionIndex < elementObj.options.length; optionIndex++) {
										if(elementObj.options[optionIndex].value == paramVal) {
												elementObj.options[optionIndex].selected = true;
												break;
										}
								}
						}
				}
        
				if (elementObj.type=="checkbox") {
						if (elementObj.value==paramVal) {
								elementObj.checked = true;
						}
				}
        
				if (elementObj.type=="radio" ) {
						if (elementObj.value==paramVal) {
								elementObj.checked = true;
						}
				}
        
				if (elementObj.type=="select-multiple" ) {
						if(elementObj.options) {
								for (var optionIndex=0; optionIndex < elementObj.options.length; optionIndex++) {
										if(elementObj.options[optionIndex].value == paramVal) {
												elementObj.options[optionIndex].selected = true;
												break;
										}
								}
						}
				}
        
				if (elementObj.type=="hidden" ) {
						elementObj.value=paramVal;
				}
   	}
	}
}



function xmlPostBeanArrayReturn(strActionCode,paramArray,preFunc,postFunc,alertFunc) 
{
    xmlPostWithReturn(strActionCode,"",paramArray,preFunc,beanListpostFunc,alertFunc);
    function beanListpostFunc()
    {
        var xmlString = xmlHttp.responseText ;
        try { //Internet Explorer  
              xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
              xmlDoc.async="false";
              xmlDoc.loadXML(xmlString);
          }
        catch(e)  {
          try { //Firefox, Mozilla, Opera, etc. 
              parser=new DOMParser();
              xmlDoc=parser.parseFromString(xmlString,"text/xml");
          }
          catch(e)  {
              alert(e.message);
              return;
          }
        }
        function beanValAdd (value,key){
            this.value = value;
            this.key = key;
        }
        
        var beanValues = new Array();
        var beanList = new Array();
        var beanNode = xmlDoc.getElementsByTagName("bean-data");
          for(var bNode = 0; bNode < beanNode.length; bNode++){
                var  paramNode = beanNode(bNode).childNodes;
                beanValues = new Array();
                for(var pNode = 0; pNode < paramNode.length; pNode++){
                         key = paramNode(pNode).childNodes(0).firstChild.text;
                         value = paramNode(pNode).childNodes(1).text;
                         beanValues[pNode] = new beanValAdd(value,key);		 
                }
                beanList[bNode] = beanValues;
           }
        
        if (postFunc!=null && postFunc!="")
                postFunc(beanList);
    }
}

function xmlPostWithReturn(strActionFlag,strMethodName,paramArray,preFunc,postFunc,alertFunc) 
{
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
      strURL = "mainController.do?actionFlag="+strActionFlag+"&methodFlag="+strMethodName;
      for(i=0;i<paramArray.length;i++)
      {
    	  //alert(paramArray[i].name);
    	 //alert(paramArray[i].value);
    	  strURL = strURL + "&"+ encodeURIComponent(paramArray[i].name) + "=" + encodeURIComponent(paramArray[i].value);
      }
      
      
       // if (ajaxImageDisplayFlag == 1)
        //{
//        	formSubmittedFlag = true;
    		//  popHiddenLayer('hiddenSubmitLayer');
        // }
        
      if (preFunc!=null && preFunc!="")
        preFunc();
            
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;            
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXRequest(postFunc,alertFunc);
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      //alert(strURL);
      xmlHttp.open('POST', strURL, false);
      xmlHttp.send(null);
      return true;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}

function xmlPost(strActionCode,paramArray) 
{
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
      strURL = "frontController.do?actionCode="+strActionCode;      
      for(i=0;i<paramArray.length;i++)
      {
        strURL = strURL + "&"+ encodeURIComponent(paramArray[i].name) + "=" + encodeURIComponent(paramArray[i].value);
      }
      if (ajaxImageDisplayFlag == 1)
        {
    	  popUnHideLayer('hiddenLayer');
        }
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXRequest('','');
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.send(null);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}


function processAJAXRequest(postFunc,alertFunc)
{
  if(xmlHttp.getResponseHeader("Content")=="XML")
  {  
      var xmlObj = xmlHttp.responseXML;
      readXML(xmlObj);
      if (postFunc!=null && postFunc!="")
        postFunc();
  }
  else if(xmlHttp.getResponseHeader("Content")=="Message")
  {
     alert((xmlHttp.responseText).substring(7));
    if (alertFunc!=null && alertFunc!="")
        alertFunc();
  }else if(xmlHttp.getResponseHeader("Content")=="OpenPage")
  {
//location.href =xmlHttp.getResponseHeader("ContextPath")+'/'+ xmlHttp.responseText).substring(19);
  }else if(xmlHttp.getResponseHeader("Content")=="DoNothing")
  {
// alert("Do Nothing")  Pilot phase over-Mohan Commanded
  }  
  else
  {
      alert("Your Session Expired please re-login..!");
  }
  
        
}


//  Object status integer:
//  0 = uninitialized
//  1 = loading
//  2 = loaded
//  3 = interactive
//  4 = complete
function XMLReqState()
{
  if(xmlHttp!=null && (xmlHttp.readyState!="undefined"))
    return xmlHttp.readyState;
}

function getParamString(formObj)
{
    var paramString = "";
    for (i = 0; i < formObj.elements.length; i++ )
    {
        elementObj = formObj.elements[i];
        if(elementObj.name!="undefined" && elementObj.name!="")
        {
          if ((elementObj.type == "text" || elementObj.type == "textarea" || elementObj.type == "hidden" )&& elementObj.value!="")
          {   
              paramString = paramString + "&" + encodeURIComponent(elementObj.name) + "=" + encodeURIComponent(elementObj.value);
          }
          else if ((elementObj.type == "select-one") && ((elementObj.options.length)>0))
          {
            if (elementObj.selectedIndex != -1)
            {
              paramString = paramString + "&" + encodeURIComponent(elementObj.name) + "=" + encodeURIComponent(elementObj.options[elementObj.selectedIndex].value);
            }
          }
          else if (elementObj.type == "select-multiple")
          {
              for (var optionIndex=0; optionIndex < elementObj.options.length; optionIndex++) 
              {
                if(elementObj.options[optionIndex].selected)
                {
                    paramString = paramString + "&" + encodeURIComponent(elementObj.name) + "=" + encodeURIComponent(elementObj.options[optionIndex].value);
                }
              }
          }
          else if ((elementObj.type == "radio" || elementObj.type == "checkbox") && elementObj.checked)
          { 
              paramString = paramString + "&" + encodeURIComponent(elementObj.name) + "=" + encodeURIComponent(elementObj.value);
          }
        }
    }
    return paramString;
}

function xmlPostIntSave(strActionCode,formObj) 
{
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
      strURL = "frontController.do?actionCode="+strActionCode;      
      queryString = getParamString(formObj);
      if (ajaxImageDisplayFlag == 1)
        {
        crtDynamicDiv();
        document.getElementById('blankImage').style.display = 'block';
        document.getElementById('rotaterImage').style.display = 'block';
        }
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXRequest();
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      xmlHttp.send(queryString);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}

// Added By Mohan (181988), For Posting the Multiselect Combo Data

function xmlPostMultiSelectComboData(strActionCode,elementObj) 
{
    xmlHttp = GetXmlHttpObject();
    var queryString = "";
    if (xmlHttp) 
    {
    strURL = "frontController.do?actionCode="+strActionCode;      
    for (var optionIndex=0; optionIndex < elementObj.options.length; optionIndex++) 
    {
      if(elementObj.options[optionIndex].selected)
      {
          queryString = queryString + "&" + encodeURIComponent(elementObj.name) + "=" + encodeURIComponent(elementObj.options[optionIndex].value);
      }
    }     
    
    if (ajaxImageDisplayFlag == 1)
        {
        crtDynamicDiv();
        document.getElementById('blankImage').style.display = 'block';
        document.getElementById('rotaterImage').style.display = 'block';
        }
    xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXRequest();
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      xmlHttp.send(queryString);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}


// Added By Mohan (181988), For Saving Data In Database- With Pre Post and Alert Function

function xmlPostSaveData(strActionCode,formObj,preFunc,postFunc,alertFunc) 
{
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
    strURL = "frontController.do?actionCode="+strActionCode;      
    queryString = getParamString(formObj);
    if (ajaxImageDisplayFlag == 1)
        {
        crtDynamicDiv();
        document.getElementById('blankImage').style.display = 'block';
        document.getElementById('rotaterImage').style.display = 'block';
        }
      if (preFunc!=null && preFunc!="")
        preFunc();
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXRequest(postFunc,alertFunc);
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      xmlHttp.send(queryString);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}



function xmlPostGetListBox(strActionCode,lb_Type,lbObj) 
{
    
    var cmbLength = lbObj.length;
    for (i = cmbLength; i > 0;   i--)  
    {
       lbObj.options.remove(i);
    }
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
      strURL = "frontController.do?actionCode="+strActionCode+"ListBoxType="+encodeURIComponent(lb_Type);
      if (ajaxImageDisplayFlag == 1)
        {
        crtDynamicDiv();
        document.getElementById('blankImage').style.display = 'block';
        document.getElementById('rotaterImage').style.display = 'block';
        }
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXComboRequest(lbObj);
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
      }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.send(null);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}

// for getting data from any table using Controllers to populate combo
function xmlPostGetCombo(strActionCode,paramArray,lbObj) 
{
    //Remove the combo options
    var cmbLength = lbObj.length;
    for (i = cmbLength; i > 0;   i--)  
    {
       lbObj.options.remove(i);
    }
    xmlHttp = GetXmlHttpObject();
    if (xmlHttp) 
    {
      strURL = "frontController.do?actionCode="+strActionCode;
      for(i=0;i<paramArray.length;i++)
      {
        strURL = strURL + "&"+ encodeURIComponent(paramArray[i].name) + "=" + encodeURIComponent(paramArray[i].value);
      }
      if (ajaxImageDisplayFlag == 1)
        {
        crtDynamicDiv();
        document.getElementById('blankImage').style.display = 'block';
        document.getElementById('rotaterImage').style.display = 'block';
        }
      xmlHttp.onreadystatechange = function()
      {
          if (xmlHttp.readyState == 4 || xmlHttp.readyState=="complete")
          {
            sessionflag = false;
            sessionTimeout();
            if (xmlHttp.status == 200) //Numeric code returned by server, such as 404 for "Not Found" or 200 for "OK"
            {
              processAJAXComboRequest(lbObj);
            }
            else
            {
              alert("There was a problem retrieving the XML data:\n" + xmlHttp.statusText);
            }
          }
    }
      xmlHttp.open('POST', strURL, true);
      xmlHttp.send(null);
      return null;
    }
    else
    {
        alert("Browser is not supporting XML");
    }
}

function processAJAXComboRequest(comboObj)
{
//  alert(xmlHttp.getResponseHeader("Content"));
  if(xmlHttp.getResponseHeader("Content")=="XML")
  {
      var xmlObj = xmlHttp.responseXML;
      populateCombo(xmlObj,comboObj);
  }else if(xmlHttp.getResponseHeader("Content")=="Message")
  {
    alert((xmlHttp.responseText).substring(7));
  }
  else
  {
    if(xmlHttp.getResponseHeader("ContextPath")!=null)
      location.href = xmlHttp.getResponseHeader("ContextPath")+"/Error/send.jsp?VastisMsg=System Error.Please contact Database Administrator.";
    else
      alert("Some Error Occured!")
  }
   if (ajaxImageDisplayFlag == 1) {
        document.getElementById('blankImage').style.display = 'none';
        document.getElementById('rotaterImage').style.display = 'none';
        }
}


function populateCombo(xmldoc,comboObj)
  {
    xmlObj=xmldoc.documentElement;
    var root = xmlObj.getElementsByTagName('param');
    for (var iNode = 0; iNode < root.length; iNode++) 
    {
      var node = root[iNode];
      if(node.childNodes(1).firstChild!=null)
      {
        var oOption = document.createElement("option");         
        oOption.value=node.childNodes(0).firstChild.text;
        oOption.text=node.childNodes(1).firstChild.text;
        comboObj.add(oOption); 
      }
    }
  }
  
  
function displayLoading()
  {
		var objLoad = document.getElementById("loading");    
		objLoad.style.display="block";
		objLoad.style.visibility="visible";
		var x = (document.body.offsetWidth)/2 + document.body.scrollLeft;
		var y = (document.body.offsetHeight)/2 + document.body.scrollTop;
                objLoad.style.left=x-300;
		objLoad.style.top=y;
    }
  

function hideLoading()
  {
  document.getElementById("loading").style.display="none";
  }


function crtDynamicDiv(){

dv = document.createElement('div'); 
dv.setAttribute('id',"blankImage");    
dv.className="forBlankImageToBlur";
dv.innerHTML='&nbsp;';
document.forms[0].appendChild(dv);

dv = null;
dv = document.createElement('div'); 
dv.setAttribute('id',"rotaterImage");    
dv.className="forAjaxImage";
dv.innerHTML='&nbsp;';
document.forms[0].appendChild(dv);
}