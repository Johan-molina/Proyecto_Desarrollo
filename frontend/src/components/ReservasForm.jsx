import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, Row, Col } from 'react-bootstrap';
import { reservasService } from '../services/api';

const ReservasForm = ({ show, handleClose, reserva, onSave }) => {
    const [formData, setFormData] = useState({
        nombreCliente: '',
        fechaEntrada: '',
        fechaSalida: '',
        email: '',
        telefono: ''
    });
    const [validated, setValidated] = useState(false);

    // useEffect corregido - sin dependencia circular
    useEffect(() => {
        if (reserva) {
            // Si estamos editando, cargar los datos de la reserva
            setFormData({
                nombreCliente: reserva.nombreCliente || '',
                fechaEntrada: reserva.fechaEntrada ? new Date(reserva.fechaEntrada).toISOString().slice(0, 16) : '',
                fechaSalida: reserva.fechaSalida ? new Date(reserva.fechaSalida).toISOString().slice(0, 16) : '',
                email: reserva.email || '',
                telefono: reserva.telefono || ''
            });
        } else {
            // Si es nueva reserva, limpiar el formulario
            setFormData({
                nombreCliente: '',
                fechaEntrada: '',
                fechaSalida: '',
                email: '',
                telefono: ''
            });
        }
    }, [reserva, show]); // Dependencias correctas: reserva y show

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const form = e.currentTarget;

        if (form.checkValidity() === false) {
            e.stopPropagation();
            setValidated(true);
            return;
        }

        // Validar que fechaSalida sea posterior a fechaEntrada
        if (new Date(formData.fechaSalida) <= new Date(formData.fechaEntrada)) {
            alert('La fecha de salida debe ser posterior a la fecha de entrada');
            return;
        }

        try {
            // Preparar datos para enviar al backend
            const reservaData = {
                nombreCliente: formData.nombreCliente,
                fechaEntrada: new Date(formData.fechaEntrada).toISOString(),
                fechaSalida: new Date(formData.fechaSalida).toISOString(),
                email: formData.email || null,
                telefono: formData.telefono || null
            };

            if (reserva) {
                // Actualizar reserva existente
                await reservasService.update(reserva.id, { ...reservaData, id: reserva.id });
                console.log('Reserva actualizada:', reserva.id);
            } else {
                // Crear nueva reserva
                await reservasService.create(reservaData);
                console.log('Nueva reserva creada');
            }

            onSave(); // Notificar al padre que se guardó
            handleClose(); // Cerrar el modal
        } catch (error) {
            console.error('Error detallado:', error);
            console.error('Respuesta del servidor:', error.response?.data);
            alert('Error al guardar la reserva: ' + (error.response?.data?.title || error.message));
        }
    };

    return (
        <Modal show={show} onHide={handleClose} size="lg" backdrop="static">
            <Modal.Header closeButton>
                <Modal.Title>
                    {reserva ? '✏️ Editar Reserva' : '➕ Nueva Reserva'}
                </Modal.Title>
            </Modal.Header>
            <Form noValidate validated={validated} onSubmit={handleSubmit}>
                <Modal.Body>
                    <Form.Group className="mb-3">
                        <Form.Label>Nombre del Cliente *</Form.Label>
                        <Form.Control
                            type="text"
                            name="nombreCliente"
                            value={formData.nombreCliente}
                            onChange={handleChange}
                            required
                            placeholder="Ej: María García"
                        />
                        <Form.Control.Feedback type="invalid">
                            El nombre del cliente es requerido
                        </Form.Control.Feedback>
                    </Form.Group>

                    <Row>
                        <Col md={6}>
                            <Form.Group className="mb-3">
                                <Form.Label>Fecha de Entrada *</Form.Label>
                                <Form.Control
                                    type="datetime-local"
                                    name="fechaEntrada"
                                    value={formData.fechaEntrada}
                                    onChange={handleChange}
                                    required
                                />
                                <Form.Control.Feedback type="invalid">
                                    La fecha de entrada es requerida
                                </Form.Control.Feedback>
                            </Form.Group>
                        </Col>
                        <Col md={6}>
                            <Form.Group className="mb-3">
                                <Form.Label>Fecha de Salida *</Form.Label>
                                <Form.Control
                                    type="datetime-local"
                                    name="fechaSalida"
                                    value={formData.fechaSalida}
                                    onChange={handleChange}
                                    required
                                />
                                <Form.Control.Feedback type="invalid">
                                    La fecha de salida es requerida
                                </Form.Control.Feedback>
                            </Form.Group>
                        </Col>
                    </Row>

                    <Row>
                        <Col md={6}>
                            <Form.Group className="mb-3">
                                <Form.Label>Correo Electrónico</Form.Label>
                                <Form.Control
                                    type="email"
                                    name="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                    placeholder="ejemplo@correo.com"
                                />
                                <Form.Text className="text-muted">
                                    Opcional
                                </Form.Text>
                            </Form.Group>
                        </Col>
                        <Col md={6}>
                            <Form.Group className="mb-3">
                                <Form.Label>Teléfono</Form.Label>
                                <Form.Control
                                    type="tel"
                                    name="telefono"
                                    value={formData.telefono}
                                    onChange={handleChange}
                                    placeholder="123456789"
                                />
                                <Form.Text className="text-muted">
                                    Opcional
                                </Form.Text>
                            </Form.Group>
                        </Col>
                    </Row>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancelar
                    </Button>
                    <Button variant="success" type="submit">
                        {reserva ? 'Actualizar Reserva' : 'Guardar Reserva'}
                    </Button>
                </Modal.Footer>
            </Form>
        </Modal>
    );
};

export default ReservasForm;