import React, { Component } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import { FormControlProps } from "react-bootstrap/FormControl";
import Modal from "react-bootstrap/Modal";
import { connect } from "react-redux";
import { ThunkDispatch } from "redux-thunk";
import { ITodo } from "../model/todo";
import {
  closeAddTodoModal,
  ITodoAction,
  postTodo,
  updatePendingTodo
} from "../redux/actions";
import { ITodoState } from "../redux/reducer";

interface IAddTodoModalProps {
  show: boolean;
  todo: ITodo;
  dispatch: ThunkDispatch<ITodoState, undefined, ITodoAction>;
}

class AddTodoModal extends Component<IAddTodoModalProps> {
  public render() {
    const userOptions = new Array(10)
      .fill(0)
      .map((_, index) => <option key={index}>{index + 1}</option>);

    return (
      <Modal show={this.props.show} onHide={this.handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Add new Todo</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="formUserId">
              <Form.Label>User ID</Form.Label>
              <Form.Control
                as="select"
                value={this.props.todo.userId.toString()}
                onChange={(e: React.FormEvent<EventTarget>) =>
                  this.handleUserIdChange((event && event.target) || null)
                }
              >
                {userOptions}
              </Form.Control>
            </Form.Group>
            <Form.Group controlId="formTodoTitle">
              <Form.Label>Task</Form.Label>
              <Form.Control
                type="input"
                placeholder="Describe the task that should be done."
                value={this.props.todo.title}
                onChange={(e: React.FormEvent<FormControlProps>) =>
                  this.handleTitleChange((event && event.target) || null)
                }
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={this.handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={this.handleSave}>
            Create
          </Button>
        </Modal.Footer>
      </Modal>
    );
  }

  private handleClose = () => this.props.dispatch(closeAddTodoModal());

  private handleSave = () => this.props.dispatch(postTodo(this.props.todo));

  private handleUserIdChange(target: EventTarget | null) {
    if (!target) {
      return;
    }

    const inputElement = target as HTMLInputElement;
    this.props.dispatch(
      updatePendingTodo(Number(inputElement.value), this.props.todo.title)
    );
  }

  private handleTitleChange(target: EventTarget | null) {
    if (!target) {
      return;
    }

    const inputElement = target as HTMLInputElement;
    this.props.dispatch(
      updatePendingTodo(this.props.todo.userId, inputElement.value)
    );
  }
}

const mapStateToProps = (store: ITodoState) => ({
  show: store.showAddTodoModal,
  todo: store.pendingTodo
});

export default connect(mapStateToProps)(AddTodoModal);