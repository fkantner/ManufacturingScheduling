function CombineOptions(opts, newopt){
  return opts.concat(newopt());
}

function FrontLoadZeros(number) {
  if (number === 0){ return "00"; }
  if (number < 10) { return "0" + number; }
  return number.toString();
}

function GetOpType(type) {
  switch (type) {
    case 0:
      return 'ShippingOp';
    case 1:
      return 'StageOp';
    case 2:
      return 'DrillOpType1';
    case 3:
      return 'DrillOpType2';
    case 4:
      return 'DrillOpType3';
    case 5:
      return 'LatheOpType1';
    case 6:
      return 'LatheOpType2';
    case 7:
      return 'CncOpType1';
    case 8:
      return 'CncOpType2';
    case 9:
      return 'CncOpType3';
    case 10:
      return 'CncOpType4';
    case 11:
      return 'PressOpType1';
    case 12:
      return 'PressOpType2';
    default:
      return 'Op Type Error';
  }
}

function ParseDay(day){
  var dayOfWeek;
  switch(day) {
    case 0: dayOfWeek = 'Su'; break;
    case 1: dayOfWeek = 'Mo'; break;
    case 2: dayOfWeek = 'Tu'; break;
    case 3: dayOfWeek = 'We'; break;
    case 4: dayOfWeek = 'Th'; break;
    case 5: dayOfWeek = 'Fr'; break;
    case 6: dayOfWeek = 'Sa'; break;
    default: dayOfWeek = 'Er';
  }
  return dayOfWeek;
}

function ParseTime(time) {
  var hour = FrontLoadZeros(Math.floor(time / 60));
  var minute = FrontLoadZeros(time % 60);

  return hour + ":" + minute;
}

export { 
  CombineOptions,
  FrontLoadZeros,
  GetOpType,
  ParseDay,
  ParseTime
};