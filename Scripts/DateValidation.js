// CREATED BY Puneet
// FOR DATE VALIDATION
// following function checks given date is valid instrument date or not
function checkInstrDate(instrdt)
{ 
   var priordate,postdate;  
   var curdate = new Date();

   var curmonth=curdate.getMonth()+1;
   var curyear=curdate.getYear();
   var curday=curdate.getDate();

   // post date checking (end of the current month)
   postdate=curmonth+"/"+getDays(curmonth-1,curyear)+"/"+curyear; 
    // date before six months
         if ( curmonth <= 6)
         {
           curmonth= (curmonth - 6)+12;
           curyear = curyear -1;
         }   
         else
	 {
           curmonth = curmonth - 6;
         }
 
         priordate=curmonth+"/"+curday+"/"+curyear; 
         if (  (Date.parse(priordate) < Date.parse(tempdate))  && 
                   (Date.parse(postdate) >= Date.parse(tempdate)) )
         {
           return true;
         }
         else
         {
          alert("The Instrument's(Cheque/Banker's Cheque/Demand Draft) validity period(6 months) has expired !!!");
           return false;
         }
} 
function getDays(m,y)
{        
    if(m==01||m==03||m==05||m==07||m==08||m==10||m==12)	
    {		
        var dmax = 31;					
        return dmax;	        	
    }	
    else if (m==04||m==06||m==09||m==11)	
    {        	
        var dmax = 30;				
        return dmax;		  	
    }	
    else	
    {		
        if((y%400==0) || (y%400==0 && y%100!=0))		
        {			
            var dmax = 29;						
            return dmax;		
        }                
        else                 
        {                    
            var dmax = 28;				                
        }
    }
    alert("hii " +dmax);
    return dmax;	
}	

function checkdate(objName)
{
    var datefield = objName;
    
    if (objName.value != 'dd/mm/yyyy' && objName.value != '')
    {
        if (chkdate(objName) == false)
        {
            datefield.select();
            alert("Please fill in dd/mm/yyyy format");
            datefield.value = "";
            datefield.select();
            datefield.focus();
        }
        else 
        {  
            return true;
        }
    }
}

function chkdate(objName) 
{	
    var strDate;
    var strDateArray;
    var strDay;
    var strMonth;
    var strYear;
    var intday;
    var intMonth;
    var intYear;
    var booFound = false;
    var datefield = objName;
    var strSeparatorArray = new Array("-"," ","/",".");
    var intElementNr;
    var err = 0;
//    alert("Server Date :: "+serverDate);
//    var serverDateArray = serverDate.split("/");
//    var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
    var comparedate;
    var strMonthArray = new Array(12);
    strMonthArray[0] = "Jan";
    strMonthArray[1] = "Feb";
    strMonthArray[2] = "Mar";
    strMonthArray[3] = "Apr";
    strMonthArray[4] = "May";
    strMonthArray[5] = "Jun";
    strMonthArray[6] = "Jul";
    strMonthArray[7] = "Aug";
    strMonthArray[8] = "Sep";
    strMonthArray[9] = "Oct";
    strMonthArray[10] = "Nov";
    strMonthArray[11] = "Dec";
//    alert(datefield.value);
    strDate = datefield.value;
    
    if (strDate.length < 6) 
    {
        return false;
    }
    for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
    {
        if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) 
        {
            strDateArray = strDate.split(strSeparatorArray[intElementNr]);
            if (strDateArray.length != 3) 
            {
            err = 1;
            return false;
            }
            else 
            {
                if(isDateNumeric(strDateArray[0]))
                    strDay =strDateArray[0];
                else return false;
                if(isDateNumeric(strDateArray[1]))
                    strMonth = strDateArray[1];
                else return false;
                if(isDateNumeric(strDateArray[2]))
                    strYear = strDateArray[2];
                else return false;
            }
            booFound = true;
        }
    }
    if (booFound == false) 
    {
        if (strDate.length>5) 
        {
            if(isDateNumeric(strDate.substr(0, 2)))
                strDay =strDate.substr(0, 2);
            else return false;
            if(isDateNumeric(strDate.substr(2, 2)))
                strMonth = strDate.substr(2, 2);
            else return false;
            if(isDateNumeric(strDate.substr(4)))
                strYear = strDate.substr(4);
            else return false;
        
        }
    }
    
    if (strYear.length == 3 ||strYear.length > 4) 
    {
        return false;
    }
    if (strYear.length == 2) 
    {
        strYear = '20' + strYear;
    }
    
    
    intday = parseInt(strDay, 10);
    if (isNaN(intday)) 
    {
        err = 2;
        return false;
    }
    intMonth = parseInt(strMonth, 10);
    if (isNaN(intMonth)) 
    {
        for (i = 0;i<12;i++) 
        {
            if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) 
            {
                intMonth = i+1;
                strMonth = strMonthArray[i];
                i = 12;
            }
        }
        if (isNaN(intMonth)) 
        {
            err = 3;
            return false;
        }
    }
    
    intYear = parseInt(strYear, 10);
    if (isNaN(intYear)) 
    {
        err = 4;
        return false;
    }
    if (intMonth>12 || intMonth<1) 
    {
        err = 5;
        return false;
    }
    if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) 
    {
        err = 6;
        return false;
    }
    if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) 
    {
        err = 7;
        return false;
    }
    if (intMonth == 2) 
    {
        if (intday < 1) 
        {
            err = 8;
            return false;
        }
        if (LeapYear(intYear) == true) 
        {
            if (intday > 29) 
            {
                err = 9;
                return false;
            }
        }
        else 
        {        
            if (intday > 28) 
            {
                err = 10;
                return false;
            }
        }
    }
    
    tempdate=strDay +"/"+intMonth+"/"+strYear;         
    //     datefield.value = strDay +"/"+intMonth+"/"+strYear;
    datefield.value = strDay +"/"+strMonth+"/"+strYear;
    
    return true;
}

function valFutureDate(from)
{ 
    
  if (from.value != '')
  {
    
   var passmonth;
   var passyear;
   var passday;  
   
   var serverDateArray = serverDate.split("/");
   var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
   //alert("Server Date is "+curdate+ "  and pc date is  "+new Date());
   var curmonth=parseInt(curdate.getMonth()+1);
   var curyear=parseInt(curdate.getYear());
   var curday=parseInt(curdate.getDate());
   var todayDate = curday+"/"+curmonth+"/"+curyear;
   if (chkdate(from) == false)
      {
      alert("Please fill in dd/mm/yyyy format");
       from.value = "";
       from.focus();
       return false;
       }
      else  
      {
      if (!fnCompareDates(from.value,todayDate))
      {
         alert("Date should not be a future date.");
         from.value="";
         from.focus();
         return false;
      }
      else
         { 
        return true;
         }
     } 
  }
}

function fnCompareDates(first,second)
{
        var val1;
        var val2;

	var k = first.indexOf("/");
        var t = first.indexOf("/",3);     
        val1 = first.substr(k+1,t-k-1) +"/"+first.substr(0,k)+"/"+first.substr(t+1,first.length);
      
	k = second.indexOf("/");
	t = second.indexOf("/",3);
	val2 = second.substr(k+1,t-k-1) +"/"+second.substr(0,k)+"/"+second.substr(t+1,second.length);
	
        if (Date.parse(val1) > Date.parse(val2))
         { 
          // alert("in if");
           return false; 
        }
        else
        { 
          //alert("in else");
          return true;           
        }
}
function fnCompareDatesWithToday(first,second)
{
        var val1;
        var val2;

	var k = first.indexOf("/");
        var t = first.indexOf("/",3);     
        val1 = first.substr(k+1,t-k-1) +"/"+first.substr(0,k)+"/"+first.substr(t+1,first.length);
      
	k = second.indexOf("/");
	t = second.indexOf("/",3);
	val2 = second.substr(k+1,t-k-1) +"/"+second.substr(0,k)+"/"+second.substr(t+1,second.length);
	
        if (Date.parse(val1) >= Date.parse(val2))
         { 
          // alert("in if");
           return false; 
        }
        else
        { 
          //alert("in else");
          return true;           
        }
}
function monthStartDate(date)
{
    var dateValue = date.value;
    
    var day = dateValue.substr(0,2);
    var month = dateValue.substr(3,2); 
    var year = dateValue.substr(6,9);
    
    date.value = "01/"+month+"/"+year;
    
//    alert(date.value);
}

function isDateNumeric(var1)
{
	var len,str,str1,i
	len=var1.length
	str=var1
	str1="0123456789"
   
    for(i=0;i<len;i++)
    {
      if((str1.indexOf(str.charAt(i)))==-1)
      {
        return false;
      }
    }
    return true
}

function LeapYear(intYear) 
{
    if (intYear % 100 == 0) 
    {
        if (intYear % 400 == 0) 
        { 
            return true; 
        }
    }
    else 
    {
        if ((intYear % 4) == 0) 
        { 
            return true; 
        }
    }
    return false;
}
function allowPastDateOnly(field)
{	
    var serverDateArray = serverDate.split("/");
    var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
    var curmonth=parseInt(curdate.getMonth()+1);
    var curyear=parseInt(curdate.getYear());
    var curday=parseInt(curdate.getDate());
    var todayDate = curday+"/"+curmonth+"/"+curyear;
    var testDate = field.value;
   if (chkdate(field) == false)
    {
    alert("Please fill in dd/mm/yyyy format");
    field.value = "";
    field.focus();
     return false;
     }
    else if(!fnCompareDates(testDate,todayDate))
    {
        alert("Date should not be a future date.")
        field.value= "";
        field.focus();
        return false;
    }
    else if(isEqual(todayDate,testDate))
    {
        alert("Date should not be today date.")    
        field.value= "";
        field.focus();
        return false;
    }
    else
    {
        return true;
    }
    
}
function isEqual(todayDate,testDate)
{
        var val1;
        var val2;

	var k = todayDate.indexOf("/");
        var t = todayDate.indexOf("/",3);     
        val1 = todayDate.substr(k+1,t-k-1) +"/"+todayDate.substr(0,k)+"/"+todayDate.substr(t+1,todayDate.length);
      
	k = testDate.indexOf("/");
	t = testDate.indexOf("/",3);
	val2 = testDate.substr(k+1,t-k-1) +"/"+testDate.substr(0,k)+"/"+testDate.substr(t+1,testDate.length);
	
        if (Date.parse(val1) == Date.parse(val2))
        {
            return true;
        }
        else
        {
            return false;
        }
}
function setFirstDate(field)
{
    var enteredDate = field.value;
    var displayDate = "01" + enteredDate.substr(2,enteredDate.length);
    field.value = displayDate;
}

function allowFutureDateOnly(from)
{ 

  if (from.value != '')
  {

   var passmonth;
   var passyear;
   var passday;  
   
   var serverDateArray = serverDate.split("/");
   var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
   //alert("Server Date is "+curdate+ "  and pc date is  "+new Date());
   var curmonth=parseInt(curdate.getMonth()+1);
   var curyear=parseInt(curdate.getYear());
   var curday=parseInt(curdate.getDate());
   var todayDate = curday+"/"+curmonth+"/"+curyear;
   if (chkdate(from) == false)
      {
      alert("Please fill in dd/mm/yyyy format");
       from.value = "";
       from.focus();
       return false;
       }
      else  
      {
      if (fnCompareDatesWithToday(from.value,todayDate))
      {
         alert("Entered date should be a Today/future date.");
         from.value="";
         from.focus();
         return false;
      }
      else
         { 
        return true;
         }
     } 
  }
}



//Input : string of date of birth in dd/mm/yyyy format
//returns age in years
function findAgeFromDOB(dobStr)
{
    var dobStrYear = dobStr.substr(6,4);
    var dobStrDay = dobStr.substr(0,2);
    var dobStrMonth = dobStr.substr(3,2);
    var dobDateObj = new Date(parseInt(dobStrYear,10), parseInt(dobStrMonth,10)-1, parseInt(dobStrDay,10) );
    var serverDateArray = serverDate.split("/");
    var todayDateObj = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
    var boolBdayOcuurred = false;
    if (dobDateObj.getMonth() < todayDateObj.getMonth())
    {
        boolBdayOcuurred = true;
        
    }
    else if((dobDateObj.getMonth() == todayDateObj.getMonth()) && (dobDateObj.getDate() <= todayDateObj.getDate()))
    {
        boolBdayOcuurred = true;
    }
    else
    {
        boolBdayOcuurred = false;
    }
    var resultAge = 0;
    if (boolBdayOcuurred) 
        resultAge = todayDateObj.getFullYear() - dobDateObj.getFullYear();
    else
        resultAge = todayDateObj.getFullYear() - dobDateObj.getFullYear() - 1;
    return resultAge;
}


//Input : HTML object that holds the Date value
//Checks that the Date is valid and formats it into proper dd/mm/yyyy format
//It calls a formatDate() function, which formats and checks the date and returns true or false
function checkDateFormat(objD1)
{
	if(!(objD1.value == '' || objD1.value == "dd/mm/yyyy"))
	{
		if(!formatDate(objD1))
		{
			alert("Please give a valid date");
			objD1.value = "";
			objD1.focus();
		}
	}
}

//Input : HTML object that holds the Date value
//Checks that the Date is valid and formats it into proper dd/mm/yyyy format 
//and returns true or false
function formatDate(obj)
{
	var dvalue = obj.value;
	var dateString = '';
	var dds,mms,yys;
	var dd,mm,yy;
	var dSeperator = new Array(".","/","-");
	var dArray;
	var iSep;
	var sepUsed = '';
	var monthDays = new Array(0,31,28,31,30,31,30,31,31,30,31,30,31);
	if(dvalue.length < 6) return false;
	if(dvalue.length > 10) return false;
	for(iSep = 0; iSep < dSeperator.length; iSep++)
	{
		if(dvalue.indexOf(dSeperator[iSep]) != -1)
		{
			sepUsed = dSeperator[iSep];
			break;
		}
	}
	if(sepUsed != '')
	{
		dArray = dvalue.split(sepUsed);
		if(dArray.length != 3)
			return false;
		if(is_Numeric(dArray[0]))
			dd = parseInt(dArray[0],10);
		else
			return false;
		if(is_Numeric(dArray[1]))
			mm = parseInt(dArray[1],10);
		else
			return false;
		if(is_Numeric(dArray[2]))
		{
			if(dArray[2].length == 2)
			dArray[2] = "20" + dArray[2];
			yy = parseInt(dArray[2],10);
		}
		else
			return false;
	}
	else
	{
		if(dvalue.length != 6 && dvalue.length != 8)
			return false;
		dds = dvalue.substr(0,2);
		mms = dvalue.substr(2,2);
		yys = dvalue.substring(4);
		if(is_Numeric(dds))
			dd = parseInt(dds,10);
		else
			return false;
		if(is_Numeric(mms))
			mm = parseInt(mms,10);
		else
			return false;
		if(is_Numeric(yys))
		{
			if(yys.length == 2)
				yys = "20" + yys;
			yy = parseInt(yys,10);
		}
		else
			return false;
	}
	
	if(mm < 1 || mm > 12)
		return false;
	if(LeapYear(yy))
		monthDays[2] = 29;
		
	if(dd < 1 || dd > monthDays[mm])
		return false;
	
	if(dd < 10)
		dateString += '0' + dd + '/';
	else
		dateString += dd + '/';
		
	if(mm < 10)
		dateString += '0' + mm + '/';
	else
		dateString += mm + '/';
	
	dateString += yy;

	obj.value = dateString;
	return true;
}

//	Category can be as :- 
//	W - Weekly
//	M - Monthly
//	Q - Quarterly
//	Y - Yearly
//	
//	input Date must be of the format dd/mm/yyyy
//	
//	Week Days start from sunday with value 0
//	if weekStartDay is 1 then This means the week is considered as Monday to sunday, so Monday is the 1st day of week.

function getEndDate(d1,category, weekStartDay)
{
	var dd = parseInt(d1.split("/")[0],10);
	var mm = parseInt(d1.split("/")[1],10);
	var yy = parseInt(d1.split("/")[2],10);
	var monthDays = new Array(0,31,28,31,30,31,30,31,31,30,31,30,31);
	var edd=0,emm=0,eyy=0;
	var endDate = '';
	if(LeapYear(yy))
		monthDays[2] = 29;
		
	if(category == "Y")
	{
		if(mm <4)
			eyy = yy;
		else
			eyy = yy + 1;
		edd = 31;
		emm = 3;
	}
	else if(category == "Q")
	{
		if(mm <= 3)
			emm = 3;
		else if(mm <= 6)
			emm = 6;
		else if(mm <= 9)
			emm = 9;
		else
			emm = 12;
			
		edd = monthDays[emm];
		eyy = yy;
	}
	else if(category == "M")
	{
		edd = monthDays[mm];
		emm = mm;
		eyy = yy;
	}
	else if(category == "W")
	{
		var tmpDate = new Date(mm + "/" + dd + "/" + yy);
		var daysToAdd = ((6 - tmpDate.getDay()) + parseInt(weekStartDay,10))%7;
		edd = dd + daysToAdd;
		emm = mm;
		eyy = yy;
		if(edd > monthDays[emm])
		{
			edd = edd - monthDays[emm];
			emm = emm + 1;
		}
		if(emm > 12)
		{
			emm = 1;
			eyy += 1;
		}
	}
	
	if(edd < 10)
		endDate += '0' + edd + '/';
	else
		endDate += edd + '/';
		
	if(emm < 10)
		endDate += '0' + emm + '/';
	else
		endDate += emm + '/';
	
	endDate += eyy;
	return endDate;
}


//	This is the wrapper function over the getEndDate, it will also check that the given date should be a start date for the 
//	Date category mentioned.

function getEndDateCheckStart(objDate,category, weekStartDay)
{
	var startDate = objDate.value;
	var endDate;
	var weekDay = new Array('Sunday', 'Monday','Tuesday','Wednesday','Thursday','Friday','Saturday');
	var dd = parseInt(startDate.split("/")[0],10);
	var mm = parseInt(startDate.split("/")[1],10);
	var yy = parseInt(startDate.split("/")[2],10);
	var wrongDate = false;
	if(category == "Y")
	{
		if(dd != 1 || mm != 4)
		{
			wrongDate = true;
			alert('The Date should be a starting Date of the Year');
		}
	}
	else if(category == "Q")
	{
		if(dd != 1 || !(mm == 4 || mm == 7 || mm == 10 || mm == 1))
		{
			wrongDate = true;
			alert('The Date should be a starting Date of the Quarter');
		}
	}
	else if(category == "M")
	{
		if(dd != 1)
		{
			wrongDate = true;
			alert('The Date should be a starting Date of the Month');
		}
	}
	else if(category == "W")
	{
		var tmpDate = new Date(mm + '/' + dd + '/' + yy);
		if(tmpDate.getDay() != weekStartDay)
		{
			wrongDate = true;
			alert("The Date should be a " + weekDay[weekStartDay] +"' day of the week");
		}
	}
	if(!wrongDate)
	{
		return getEndDate(startDate,category, weekStartDay);
	}
	else
	{
		objDate.value = "";
                return "";
	}
}

//following function will compare entered date with System Date.
//Pass second argument as a system date.
//In other functions, Sysdate is fetched using JavaScript Date object, which is not correct.
//Because the JavaScript Date object is dependent on Client System. 
function chkWithSysDate(inputDt,SysDt)
{
  if( formatDate(inputDt) && formatDate(SysDt) )
    if ( ! ModeCompareDates(inputDt,SysDt,'2',inputDt,'','','noalert') )
    {
      alert("Entered date Should not be future Date.");
      inputDt.select();
    }
}

//following function will compare entered date with System Date.
//Pass second argument as a system date.
//In other functions, Sysdate is fetched using JavaScript Date object, which is not correct.
//Because the JavaScript Date object is dependent on Client System. 
function chkInputDateNotPastDate(inputDt)
{
  
  var serverDateArray = serverDate.split("/");
   var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
   //alert("Server Date is "+curdate+ "  and pc date is  "+new Date());
   var curmonth=parseInt(curdate.getMonth()+1);
   var curyear=parseInt(curdate.getYear());
   var curday=parseInt(curdate.getDate());
   var SysDt = curday+"/"+curmonth+"/"+curyear;
   
   var SysDtObj = new Object();
    SysDtObj.value = SysDt;
   
  if( formatDate(inputDt) && formatDate(SysDtObj) )
    if ( ! ModeCompareDates(inputDt,SysDtObj,'3',inputDt,'','','noalert') )
    {
      alert("Entered date Should not be a past Date.");
      //setTimeout(function(){inputDt.focus()}, 10);
      inputDt.value="";
      return false;
    }
    return true;
}

//Function for comparing two dates
// conditions for MODE can be
//-1 = date1 > date2
//0  = date1 = date2
//1  = date1 < date2
//2  = date1 <= date2
//3  = date1 >= date2
//
//  Flag is used to decide whether to show alert or not...

function ModeCompareDates(obj1,obj2, mode,objfocus,labeldate1,labeldate2, flag)
{
  var msgs = new Array();
	var d1, d2, frmdt, todt;
	msgs[0] = labeldate1 + " must be greater than " + labeldate2;
	msgs[1] = labeldate1 + " must be equal to " + labeldate2;
	msgs[2] = labeldate1 + " must be less than " + labeldate2;
	msgs[3] = labeldate1 + " must be less than or equal to " + labeldate2;
	msgs[4] = labeldate1 + " must be greater than or equal to " + labeldate2;
  
  //alert("d1 :" + obj1.value + "\ndate2 :" + obj2.value + "\nmode :" + mode + "\nlabeldate1 :" + labeldate1 + "\nlabeldate2 :" + labeldate2);
  
  if(obj1 && obj2 && (obj1.value.length != 0 && obj2.value.length != 0) )
	{
		d1 = obj1.value;
		d2 = obj2.value;
		frmdt = new Date(d1.split("/")[2],d1.split("/")[1],d1.split("/")[0]);
		todt = new Date(d2.split("/")[2],d2.split("/")[1],d2.split("/")[0]);
		var status = -1;
		if ( todt.getTime() > frmdt.getTime() )
		{
			status = 1;
		}
		else if( todt.getTime() == frmdt.getTime() )
		{
			status = 0;
		}
		if(mode == status)			
			return true;
		else if(mode == 2 && (status == 1 || status == 0) )
			return true;		
		else if(mode == 3 && (status == -1 || status == 0) )
			return true;		
		else
		{
                      if(flag != "noalert")
                      {
                        alert(msgs[parseInt(mode) + 1]);
                        setTimeout(function(){objfocus.focus()}, 10);
                        objfocus.value="";
                      }
		}
		return false;
	}
	return true;
}


//	Category can be as :- 
//	M - Monthly
//	Q - Quarterly
//	Y - Yearly
//	
//	input Date must be in the format dd/mm/yyyy
//      startEnd variable should have value 'start' or 'end'
//	Following function checks the Date with the respective category, either start or end

function checkDateStartEnd(dtVal ,cat, startEnd)
{
    var dd = parseInt(dtVal.split("/")[0],10);
    var mm = parseInt(dtVal.split("/")[1],10);
    var yy = parseInt(dtVal.split("/")[2],10);
    var monthDays = new Array(0,31,28,31,30,31,30,31,31,30,31,30,31);
    if(LeapYear(yy))
        monthDays[2] = 29;
                
    if(cat == "M")
    {
        if(startEnd == 'start')
        {
            if(dd != 1)
                return false;
        }
        else
        {
            if(dd != monthDays[mm])
                return false;
        }
    }
    else if(cat == "Q")
    {
        if(startEnd == 'start')
        {
            if(dd != 1 || !(mm == 4 || mm == 7 || mm == 10 || mm == 1))
                return false;
        }
        else
        {
            if(dd != monthDays[mm] || !(mm == 3 || mm == 6 || mm == 9 || mm == 12))
                return false;
        }
    }
    else if(cat == "Y")
    {
        if(startEnd == 'start')
        {
            if(dd != 1 || mm != 4)
                return false;
        }
        else
        {
            if(mm != 3 || dd != monthDays[3])
                return false;
        }
    }
    return true;
}

// Following is the wrapper function over checkDateStartEnd to give alert message also
function checkDateStartEndAlert(obj ,cat, startEnd)
{
    var category;
    if(cat == 'M')
        category = 'Month';
    else if(cat == 'Q')
        category = 'Quarter';
    else if(cat == 'Y')
        category = 'Year';
        
    if(!checkDateStartEnd(obj.value,cat,startEnd))
    {
        alert('The date should be ' + startEnd + ' Date of ' + category);
        obj.value="";
        obj.focus();
        return false;
    }
    else
    {
        return true;    
    }
}

function is_Numeric(numVal)
{
	var len,str,str1,i
	len=numVal.length
	str=numVal
	str1="0123456789"
	for(i=0;i<len;i++)
	{
		if((str1.indexOf(str.charAt(i)))==-1)
		{
			return false;
		}
	}
	return true
}


function chkHearingDate(fieldObj)
{ 
  if (fieldObj.value != '')
  {
       var passmonth;
       var passyear;
       var passday;  
       
       var serverDateArray = serverDate.split("/");
       var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
       var curmonth=parseInt(curdate.getMonth()+1);
       var curyear=parseInt(curdate.getYear());
       var curday=parseInt(curdate.getDate());
       var todayDate = curday+"/"+curmonth+"/"+curyear;
       if (chkdate(fieldObj) == false)
       {
           alert("Please fill in dd/mm/yyyy format");
           fieldObj.focus();
           return false;
        }
        else  
        {
              if (!fnCompareDates(todayDate,fieldObj.value) && !isEqual(todayDate,fieldObj.value))
              {
                 alert("Entered date should be a Today's Date or Future date.");
                 fieldObj.value="";
                 fieldObj.focus();
                 return false;
              }
              else
             { 
                  return true;
              }
         } 
  }
}

function chkFutureDate(fieldObj)
{ 
  if (fieldObj.value != '')
  {
       var passmonth;
       var passyear;
       var passday;  
       
       var serverDateArray = serverDate.split("/");
       var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
       var curmonth=parseInt(curdate.getMonth()+1);
       var curyear=parseInt(curdate.getYear());
       var curday=parseInt(curdate.getDate());
       var todayDate = curday+"/"+curmonth+"/"+curyear;
       if (chkdate(fieldObj) == false)
       {
           alert("Please fill in dd/mm/yyyy format");
           fieldObj.focus();
           return false;
        }
        else  
        {
              if (!fnCompareDt(todayDate,fieldObj.value))
              {
                 alert("Entered date should be a Future date only.");
                 fieldObj.value="";
                 fieldObj.focus();
                 return false;
              }
              else
             { 
                  return true;
              }
         } 
  }
}

function fnCompareDt(first,second)
{
        var val1;
        var val2;

	var k = first.indexOf("/");
        var t = first.indexOf("/",3);     
        val1 = first.substr(k+1,t-k-1) +"/"+first.substr(0,k)+"/"+first.substr(t+1,first.length);
      
	k = second.indexOf("/");
	t = second.indexOf("/",3);
	val2 = second.substr(k+1,t-k-1) +"/"+second.substr(0,k)+"/"+second.substr(t+1,second.length);
	
        if (Date.parse(val1) >= Date.parse(val2))
         { 
           //alert("in if");
           return false; 
        }
        else
        { 
          //alert("in else");
          return true;           
          }
}
   
    
   /**  function is used for set and check weather any period fall in  the finacial year or not
        @param frmDateObj as DD/MM/YYYY 
        @param toDateObj as DD/MM/YYYY 
        @param setToDateAuto is boolean , should be either true or false
        @param alertMsg 
        @param setToDateAuto this parameter will set the Finacial Year  end date automatic on the basis of 
        From Date if setToDateAuto==true.
        @param alertMsg if any date does not comes in any finacial year then alertMsg will  alert the 
        specifed message and  function will return false.
        
        example--- from date is 01/04/2001 
                   to date is 31/03/2002
                   will return the true.
                   
        example--- from date is 01/04/2000
                   to date is 31/03/2002
                   will return the false as 
                   to date should be 31/03/2001.
     **/


  function setnChkIsFinacialYear(frmDateObj,toDateObj,setToDateAuto,alertMsg)
   {
        if ( frmDateObj.value == null || frmDateObj.value == '' || frmDateObj.value == 'dd/mm/yyyy')
        {
            return false;
        }
        var frmDate = frmDateObj.value;
        var toDate =  toDateObj.value;
        var frmDateMonth = frmDate.split("/");
        var month = frmDateMonth[1];
        var year = frmDateMonth[2];
        var tempDate = "";
        var yearStartDate = "";
        var yearEndDate = "";
        var tempYear;
        var tempToDate = new Date();
        var toDateArray = toDate.split("/");
        var toDateFunctObj = new Date();
        
        if (Number(toDateArray[0]) == 31)
        {
            toDateFunctObj.setDate(Number(0));
        }
        toDateFunctObj.setDate(Number(toDateArray[0]));
        toDateFunctObj.setMonth(Number(toDateArray[1])-1);
        toDateFunctObj.setYear(Number(toDateArray[2]));
        
        
        if (Number (month) >= 4 && Number (month) <= 12 )
        {
            tempYear = Number(year)+1;
            yearStartDate = "01/04/"+year;
            yearEndDate = "31/03/"+tempYear;
            
            tempToDate.setDate(Number(0)); //dont set the date 31 as it will show 1  if 0 is set then it will show 31
            tempToDate.setMonth(2);
            tempToDate.setYear(tempYear);
            
        }
        else if (Number (month) >= 1 && Number (month) <= 3 )
        {
            tempYear = Number(year)-1;
            yearStartDate  ="01/04/"+tempYear;
            yearEndDate = "31/03/"+year;
            
            tempToDate.setDate(Number(0));//dont set the date 31 as it will show 1  if 0 is set then it will show 31
            tempToDate.setMonth(2);
            tempToDate.setYear(year);
        }
        
        if (setToDateAuto == true)
        {
            frmDateObj.value = yearStartDate;
            toDateObj.value = yearEndDate;
            return true;
        }
        
        if (toDateFunctObj > tempToDate )
        {
            alert(alertMsg);
            return false;
        }
        
        return true;
   }
   
    /** function is used for set and check weather any period fall in Quarter or not
        @param frmDateObj as DD/MM/YYYY 
        @param toDateObj as DD/MM/YYYY 
        @param setToDateAuto is boolean , should be either true or false
        @param alertMsg 
       
        @param setToDateAuto this parameter will set the Quarter end date automatic on the basis of 
        From Date if setToDateAuto==true.
       
        @param alertMsg if any date does not comes in any Quarter then alertMsg will alert the 
        specifed message and  function will return false.
        
        examples-- from Date 01/04/2005
                    end date 30/04/2005
                    will return the true.
                    
        examples-- from Date 01/09/2005
                    end date 31/10/2005
                    will return the false
                    as end date should be 31/12/2005.
        
        **/
  function  setnChkIsQuarterPeriod(frmDateObj,toDateObj,setToDateAuto, alertMsg)
  {
     if (frmDateObj.value == "")
     {
        toDateObj.value = "";
        return false;
     }
     var frmDate =  frmDateObj.value;
     var toDate =   toDateObj.value;
     var tempDate = "";
     var frmDateMonth = frmDate.split("/");
     var month = frmDateMonth[1];
     var year = frmDateMonth[2];
     if ( Number (month) == 4 || Number (month) == 5 || Number (month) == 6)
     {
        frmDateObj.value = '01/04/'+year; 
        tempDate = '30/06/'+year;
     }
     
     else if (Number (month) == 7 || Number (month) == 8 || Number (month) == 9)
     {
        frmDateObj.value = '01/07/'+year; 
        tempDate = '30/09/'+year;
     }
     
     else if (Number (month) == 10 || Number (month) == 11 || Number (month) == 12)
     {
        frmDateObj.value = '01/10/'+year; 
        tempDate= '31/12/'+year;
     }
     
     else if (Number (month) == 1 || Number (month) == 2 || Number (month) == 3)
     {
        frmDateObj.value = '01/01/'+year; 
        tempDate= '31/03/'+year;
     }
     
     if (setToDateAuto == true)
     {
        toDateObj.value = tempDate;
        return true;
     }
     if (tempDate != toDateObj.value)
     {
       alert(alertMsg);
       return false;
     }
     
     if(frmDateObj.value == toDateObj.value)
     {
        alert("From Date and To Date are same");
        frmDateObj.value = "";
        toDateObj.value = "";
        frmDateObj.focus;
        return false;
     }
     return true;
  }
  
  // Following function is used to convert the Postgres DB Date format into general dd/mm/yyyy format
  function dbDateToGen(dtVal)
  {
	  var genDtVal = '';
	  if(dtVal != '')
	  {
		  var tmpArray = dtVal.split('-');
		  genDtVal = tmpArray[2] + "/" + tmpArray[1] + "/" + tmpArray[0];
	  }
	  return genDtVal;
  }
  
  //this function will check for correct time in HH:MM format.....
  function validateTime(txtTime)
  {
	  var strval = txtTime.value;
	  //alert(strval);
	  	    
	  //minimum length is 5. example 01:00
	  if(strval.length < 5 || strval.length > 5)
	  {
	   alert("Invalid time. Time format should be HH:MM ");
	   return false;
	  }
	  	       
	  var pos1 = strval.indexOf(':');
	  txtTime.value = strval;
	  
	  if(pos1 < 0 )
	  {
	   alert("Invalid time. A colon(:) is missing between hour and minute.");
	   return false;
	  }
	  else if(pos1 > 2 || pos1 < 1)
	  {
	   alert("invalid time. Time format should be HH:MM ");
	   return false;
	  }
	  
	  //Checking hours
	  var horval =  trimString(strval.substring(0,pos1));
	  //alert(horval);
	  if(horval == -100)
	  {
	   alert("Invalid time. Hour should contain only integer value (0-24).");
	   return false;
	  }
	     
	  if(horval > 24)
	  {
	   alert("Invalid time. Hour can not be greater that 24.");
	   return false;
	  }
	  else if(horval < 0)
	  {
	   alert("Invalid time. Hour can not be hours less than 0.");
	   return false;
	  }
	  //Completes checking hours.

	  //Checking minutes.
	  var minval =  trimString(strval.substring(pos1+1,pos1 + 3));
	  if(minval == -100)
	  {
	   alert("Invalid time. Minute should have only integer value (0-59).");
	   return false;
	  }
	  if(minval > 59)
	  {
	     alert("Invalid time. Minute can not be more than 59.");
	     return false;
	  }   
	  else if(minval < 0)
	  {
	   alert("Invalid time. Minute can not be less than 0.");
	   return false;
	  }
	  //Checking minutes completed.  
	 return true;
}
  function trimString(str) 
  { 
       var str1 = ''; 
       var i = 0; 
       while ( i != str.length) 
       { 
           if(str.charAt(i) != ' ') str1 = str1 + str.charAt(i); i++; 
       }
       var retval = IsNumeric(str1); 
       if(retval == false) 
           return -100; 
       else 
           return str1; 
  }
  function IsNumeric(strString) 
  { 
      var strValidChars = "0123456789"; 
      var strChar; 
      var blnResult = true; 
      //var strSequence = document.frmQuestionDetail.txtSequence.value; 

      //test strString consists of valid characters listed above 

      if (strString.length == 0) 
          return false; 
      for (i = 0; i < strString.length && blnResult == true; i++) 
      { 
          strChar = strString.charAt(i); 
          if (strValidChars.indexOf(strChar) == -1) 
          { 
              blnResult = false; 
          } 
       } 
  return blnResult; 
  }
  //Added by Puneet for Exit Pass Date Check
  
  function allowExitPassDateOnly(from)
  { 
    if (from.value != '')
    {
     var passmonth;
     var passyear;
     var passday;  
     
     var serverDateArray = serverDate.split("/");
     var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
     //alert("Server Date is "+curdate+ "  and pc date is  "+new Date());
     var curmonth=parseInt(curdate.getMonth()+1);
     var curyear=parseInt(curdate.getYear());
     var curday=parseInt(curdate.getDate());
     var todayDate = curday+"/"+curmonth+"/"+curyear;
     var checkday = curday +1;
     var checkDate=checkday+"/"+curmonth+"/"+curyear;
     if (chkdate(from) == false)
        {
        alert("Please fill in dd/mm/yyyy format");
         from.value = "";
         from.focus();
         return false;
         }
        
        else if (fnCompareDt(checkDate,from.value))
        {
           alert("Gate Pass allowed only for today & Day after tommorow");
           from.value="";
           from.focus();
           return false;
        }
        else if (fnCompareDatesWithToday(from.value,todayDate))
        {
           alert("Entered date should be a Today/future date.");
           from.value="";
           from.focus();
           return false;
        }
        else
           { 
          return true;
           }
        
    }
  } 
  function allowPaySlipDate(selMonth)
  { 
     var passmonth;
     var passyear;
     var passday;  
     var serverDateArray = serverDate.split("/");
     var curdate = new Date(serverDateArray[2],serverDateArray[1]-1,serverDateArray[0]);
     //alert("Server Date is "+curdate+ "  and pc date is  "+new Date());
     var curmonth=parseInt(curdate.getMonth()+1);
     var curyear=parseInt(curdate.getYear());
     var curday=parseInt(curdate.getDate());
     var todayDate = curday+"/"+curmonth+"/"+curyear;
     var checkDate=6+"/"+curmonth+"/"+curyear;
     //alert("todayDate "+todayDate +"checkDate "+ checkDate);
     if(selMonth.substr(1,1)== curmonth-1)
     {
    	 if (fnCompareDt(todayDate,checkDate))
    	 {
    		 return true;
    	 }
    	 else
    	 { 
    		 return false;
    	 }
     }
     return false;
     } 