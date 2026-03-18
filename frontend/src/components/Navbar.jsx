import { Navbar, Nav, Container } from 'react-bootstrap';
import { Link } from 'react-router-dom';

function NavigationBar() {
    return (
        <Navbar bg="dark" variant="dark" expand="lg">
            <Container>
                <Navbar.Brand as={Link} to="/">🏨 Hotel</Navbar.Brand>
                <Nav>
                    <Nav.Link as={Link} to="/citas">Citas</Nav.Link>
                    <Nav.Link as={Link} to="/reservas">Reservas</Nav.Link>
                </Nav>
            </Container>
        </Navbar>
    );
}

export default NavigationBar;