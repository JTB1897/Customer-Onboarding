import React, { useState, useEffect } from 'react';
import './index.css';
import SignaturePad from './SignaturePad';

const API_BASE_URL = 'http://localhost:5000/api';

function App() {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    signatureData: null,
  });

  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [showForm, setShowForm] = useState(true);
  const [successfulCustomer, setSuccessfulCustomer] = useState(null);

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/customers`);
      if (response.ok) {
        const data = await response.json();
        setCustomers(data);
      }
    } catch (error) {
      console.error('Error fetching customers:', error);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSignatureChange = (signature) => {
    setFormData(prev => ({
      ...prev,
      signatureData: signature
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');

    try {
      const response = await fetch(`${API_BASE_URL}/customers`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          firstName: formData.firstName,
          lastName: formData.lastName,
          email: formData.email,
          phoneNumber: formData.phoneNumber,
          signatureData: formData.signatureData,
        }),
      });

      if (response.ok) {
        const customer = await response.json();
        setSuccessfulCustomer(customer);
        setMessage('Customer registered successfully!');
        setFormData({ firstName: '', lastName: '', email: '', phoneNumber: '', signatureData: null });
        setShowForm(false);
        await fetchCustomers();
      } else {
        const errorData = await response.json();
        setMessage(errorData.error || 'Error registering customer');
      }
    } catch (error) {
      setMessage('Error registering customer: ' + error.message);
    } finally {
      setLoading(false);
    }
  };

  const handleNewRegistration = () => {
    setShowForm(true);
    setSuccessfulCustomer(null);
    setMessage('');
  };

  return (
    <div className="container">
      <h1>Customer Onboarding System</h1>

      {message && <div className="success">{message}</div>}

      {!showForm && successfulCustomer ? (
        <div className="confirmation">
          <h2>✓ Registration Confirmed</h2>
          <p><strong>Name:</strong> {successfulCustomer.firstName} {successfulCustomer.lastName}</p>
          <p><strong>Email:</strong> {successfulCustomer.email}</p>
          <p><strong>Phone:</strong> {successfulCustomer.phoneNumber}</p>
          <p><strong>Customer ID:</strong> {successfulCustomer.id}</p>
          <p><strong>Date Created:</strong> {new Date(successfulCustomer.dateCreated).toLocaleString()}</p>
          <button onClick={handleNewRegistration}>Register New Customer</button>
        </div>
      ) : (
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="firstName">First Name *</label>
            <input
              type="text"
              id="firstName"
              name="firstName"
              value={formData.firstName}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="lastName">Last Name *</label>
            <input
              type="text"
              id="lastName"
              name="lastName"
              value={formData.lastName}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="email">Email *</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="phoneNumber">Phone Number *</label>
            <input
              type="tel"
              id="phoneNumber"
              name="phoneNumber"
              value={formData.phoneNumber}
              onChange={handleInputChange}
              required
            />
          </div>

          <SignaturePad onSignatureChange={handleSignatureChange} />

          <div className="button-group">
            <button type="submit" disabled={loading}>
              {loading ? 'Registering...' : 'Register Customer'}
            </button>
          </div>
        </form>
      )}

      {customers.length > 0 && (
        <div className="customer-list">
          <h2>Registered Customers ({customers.length})</h2>
          {customers.map(customer => (
            <div key={customer.id} className="customer-item">
              <p><strong>{customer.firstName} {customer.lastName}</strong></p>
              <p>Email: {customer.email}</p>
              <p>Phone: {customer.phoneNumber}</p>
              <p>Registered: {new Date(customer.dateCreated).toLocaleString()}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default App;
