const serverPath = "https://localhost:63728";

export const fetchTickets = async () => {
    const token = localStorage.getItem("authToken"); // Wstaw tutaj swój token
  
    try {
      const response = await fetch(serverPath + '/tickets', {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      });
  
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
  
      const data = await response.json();
      console.log('Tickets:', data);
      return data;
  
    } catch (error) {
      console.error('Error fetching tickets:', error);
    }
  };