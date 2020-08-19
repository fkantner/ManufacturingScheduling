function CombineOptions(opts, newopt){
  return opts.concat(newopt());
}

function FrontLoadZeros(number) {
  if (number === 0){ return "00"; }
  if (number < 10) { return "0" + number; }
  return number.toString();
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
  ParseDay,
  ParseTime
};