import "./assets/info.css";
import playicon from "./assets/play.svg";
import { Link } from "react-router-dom";

const Info: React.FC = () => {
  return (
    <div className="infopage-container">
      {/* Hero Section */}
      <section className="hero">
        <div className="overlay">
          <div className="hero-content">
            <h1>PrintMe</h1>
            <p>Bring all your visions to life.</p>
            <Link to="/main">
              <img src={playicon} alt="Play Icon" className="play-icon" />
            </Link>
          </div>
        </div>
      </section>

      {/* About Section */}
      <section className="about">
        <h2>What is our service about?</h2>
        <p>
        Our platform is a convenient online service that connects people who need 3D printing with those who own 3D printers.
        Here, you can either <strong>order a 3D print</strong> or <strong>offer your own 3D printer</strong> to provide printing services.
        </p>
      </section>

      {/* Additional Content */}
      <div className="info-container">
        <h4 className="fw-semibold">If You Want to Order a 3D Print</h4>
        <ul className="list-group list-group-flush">
          <li className="list-group-item">
            <strong>1. Choose a convenient location on the map</strong><br />
            Go to the “Main Page” and browse the map to find a printer near you.
          </li>
          <li className="list-group-item">
            <strong>2. Create an order</strong><br />
            Click on “Create Order” next to the printer. Fill in details, upload link of your 3D model, and choose materials.
          </li>
          <li className="list-group-item">
            <strong>3. Track your order</strong><br />
            View the order’s status and access the printer owner’s profile or chat.
          </li>
          <li className="list-group-item">
            <strong>4. Discuss delivery and payment in chat</strong><br />
            In the chat, it is necessary to discuss both the delivery and the payment for the product.
          </li>
        </ul>

        <h4 className="fw-semibold">If You Own a 3D Printer</h4>
        <ul className="list-group list-group-flush">
          <li className="list-group-item">
            <strong>1. Add your printer</strong><br />
            Fill in printer details on "Printers" page. Add your printer location to help users find you.
          </li>
          <li className="list-group-item">
            <strong>2. Wait for orders</strong><br />
            You’ll receive a notification when an order is placed.
          </li>
          <li className="list-group-item">
            <strong>3. Accept or decline orders</strong><br />
            You decide whether to proceed with the order.
          </li>
          <li className="list-group-item">
            <strong>4. Negotiate in chat</strong><br />
            In the chat, it is necessary to discuss both the delivery and the payment for the product.
          </li>
        </ul>

        <h4 className="fw-semibold">Benefits of Our Platform</h4>
        <ul className="list-group list-group-flush">
          <li className="list-group-item">
            <strong>Convenient map search</strong> — find printers/customers near you.
          </li>
          <li className="list-group-item">
            <strong>Direct chat</strong> — no middlemen, communicate freely.
          </li>
          <li className="list-group-item">
            <strong>Flexible order system</strong> — view and manage your orders easily.
          </li>
          <li className="list-group-item">
            <strong>Modern UI with dark mode</strong> — pleasant and easy to navigate.
          </li>
        </ul>

      <blockquote className="blockquote text-center mt-4">
        <p className="mb-0">
          Our goal is to make 3D printing accessible to everyone. Whether you're looking to print something or offer your services, this is your space.
        </p>
      </blockquote>
      </div>

    </div>
  );
};
//
export default Info;