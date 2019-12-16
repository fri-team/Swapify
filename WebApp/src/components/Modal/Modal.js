import React from 'react';
import './Modal.css';

const modal = props => (
    <div className="modal">
        <header className="modal__header"><h1>{props.title}</h1></header>
        <section className= "modal__content">
            {props.children}
        </section>
        <section className= "modal__actions">
            <button className="FormField__Button" onClick={props.onCancel}>Cancel</button>
        </section>
    </div>
);

export default modal;
