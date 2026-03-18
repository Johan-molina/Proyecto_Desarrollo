import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, Row, Col } from 'react-bootstrap';
import { citasService } from '../services/api';

const CitasForm = ({ show, handleClose, cita, onSave }) => {
    const [formData, setFormData] = useState({
        nombreCliente: '',
        fecha: '',
        motivo: '',
        email: '',
        telefono: ''
    });
    const [validated, setValidated] = useState(false);

    // useEffect corregido - sin dependencia circular
    useEffect(() => {
        if (cita) {
            // Si estamos editando, cargar los datos de la cita
            setFormData({
                nombreCliente: cita.nombreCliente || '',
                fecha: cita.fecha ? new Date(cita.fecha).toISOString().slice(0, 16) : '',
                motivo: cita.motivo || '',
                email: cita.email || '',
                telefono: cita.telefono || ''
            });
        } else {
            // Si es nueva cita, limpiar el formulario
            setFormData({
                nombreCliente: '',
                fecha: '',
                motivo: '',
                email: '',
                telefono: ''
            });
        }
    }, [cita, show]); // Dependencias correctas: cita y show

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

        try {
            // Preparar datos para enviar al backend
            const citaData = {
                nombreCliente: formData.nombreCliente,
                fecha: new Date(formData.fecha).toISOString(),
                motivo: formData.motivo || null,
                email: formData.email || null,
                telefono: formData.telefono || null
            };

            if (cita) {
                // Actualizar cita existente
                await citasService.update(cita.id, { ...citaData, id: cita.id });
                console.log('Cita actualizada:', cita.id);
            } else {
                // Crear nueva cita
                await citasService.create(citaData);
                console.log('Nueva cita creada');
            }

            onSave(); // Notificar al padre que se guardó
            handleClose(); // Cerrar el modal
        } catch (error) {
            console.error('Error detallado:', error);
            console.error('Respuesta del servidor:', error.response?.data);
            alert('Error al guardar la cita: ' + (error.response?.data?.title || error.message));
        }
    };

    return (
        <Modal show={show} onHide={handleClose} size="lg" backdrop="static">
            <Modal.Header closeButton>
                <Modal.Title>
                    {cita ? '✏️ Editar Cita' : '➕ Nueva Cita'}
                </Modal.Title>
            </Modal.Header>
            <Form noValidate validated={validated} onSubmit={handleSubmit}>
                <Modal.Body>
                    <Row>
                        <Col md={6}>
                            <Form.Group className="mb-3">
                                <Form.Label>Nombre del Cliente *</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="nombreCliente"
                                    value={formData.nombreCliente}
                                    onChange={handleChange}
                                    required
                                    placeholder="Ej: Juan Pérez"
                                />
                                <Form.Control.Feedback type="invalid">
                                    El nombre del cliente es requerido
                                </Form.Control.Feedback>
                            </Form.Group>
                        </Col>
                        <Col md={6}>
                            <Form.Group className="mb-3">
                                <Form.Label>Fecha y Hora *</Form.Label>
                                <Form.Control
                                    type="datetime-local"
                                    name="fecha"
                                    value={formData.fecha}
                                    onChange={handleChange}
                                    required
                                />
                                <Form.Control.Feedback type="invalid">
                                    La fecha y hora son requeridas
                                </Form.Control.Feedback>
                            </Form.Group>
                        </Col>
                    </Row>

                    <Form.Group className="mb-3">
                        <Form.Label>Motivo</Form.Label>
                        <Form.Control
                            as="textarea"
                            rows={3}
                            name="motivo"
                            value={formData.motivo}
                            onChange={handleChange}
                            placeholder="Motivo de la cita (opcional)"
                        />
                    </Form.Group>

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
                    <Button variant="primary" type="submit">
                        {cita ? 'Actualizar Cita' : 'Guardar Cita'}
                    </Button>
                </Modal.Footer>
            </Form>
        </Modal>
    );
};

export default CitasForm;