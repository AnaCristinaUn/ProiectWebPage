const calendarDates = document.querySelector('.calendar-dates');
const monthYear = document.getElementById('month-year');
const prevMonthBtn = document.getElementById('prev-month');
const nextMonthBtn = document.getElementById('next-month');

let currentDate = new Date();
let currentMonth = currentDate.getMonth();
let currentYear = currentDate.getFullYear();

const months = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December'
];

function handleDateSelection(dayElement, day, month, year){
  document.querySelectorAll('.calendar-dates div').forEach(date =>{
    date.classList.remove('current-date', 'selected-date');
  });

  dayElement.classList.add('selected-date');

  const selectedDate = new Date(year, month, day);
  const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    document.getElementById('current-date').textContent = selectedDate.toLocaleDateString('en-US', options);
}

function renderCalendar(month, year) {
  const calendarDates = document.querySelector('.calendar-dates');
    calendarDates.innerHTML = '';
    monthYear.textContent = `${months[month]} ${year}`;
  
    // Get today's date
    const today = new Date();

    // Get the first day of the month
    const firstDay = new Date(year, month, 1).getDay();
  
    // Get the number of days in the month
    const daysInMonth = new Date(year, month + 1, 0).getDate();
  
    // Create blanks for days of the week before the first day
    for (let i = 0; i < firstDay; i++) {
      const blank = document.createElement('div');
      calendarDates.appendChild(blank);
    }
  
    // Populate the days
    for (let i = 1; i <= daysInMonth; i++) {
      const dayElement = document.createElement('div');
      dayElement.textContent = i;
      calendarDates.appendChild(dayElement);

      // Highlight today's date
      if (i === today.getDate() && year === today.getFullYear() && month === today.getMonth()) {
        dayElement.classList.add('current-date');
    }
     dayElement.addEventListener('click', () => {
      handleDateSelection(dayElement, i, month, year);
  });
    }
  }
  renderCalendar(currentMonth, currentYear);

  prevMonthBtn.addEventListener('click', () => {
    currentMonth--;
    if (currentMonth < 0) {
      currentMonth = 11;
      currentYear--;
    }
    renderCalendar(currentMonth, currentYear);
  });
  
  nextMonthBtn.addEventListener('click', () => {
    currentMonth++;
    if (currentMonth > 11) {
      currentMonth = 0;
      currentYear++;
    }
    renderCalendar(currentMonth, currentYear);
  });

  function updateCurrentDate() {
    const today = new Date();
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    document.getElementById('current-date').textContent = today.toLocaleDateString('en-US', options);
}

updateCurrentDate();