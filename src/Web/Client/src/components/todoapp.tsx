import React, { Component, Dispatch } from "react";
import Button from "react-bootstrap/Button";
import Navbar from "react-bootstrap/Navbar";
import Pagination from "react-bootstrap/Pagination";
import Spinner from "react-bootstrap/Spinner";
import Table from "react-bootstrap/Table";
import { connect } from "react-redux";
import { ThunkDispatch } from "redux-thunk";
import { ITodo } from "../model/todo";
import {
  ITodoAction,
  selectUser,
  showAddTodoModal,
  modifyTodo,
  deleteTodo
} from "../redux/actions";
import { ITodoState } from "../redux/reducer";
import AddTodoModal from "./addtodomodal";
import { TodoRow } from "./todorow";
import { IUser } from "../model/user";

interface ITodoAppProps {
  isFetchingTodos: boolean;
  isFetchingUsers: boolean;
  todos: ITodo[];
  users: IUser[];
  selectedUser: number;
  dispatch: ThunkDispatch<ITodoState, undefined, ITodoAction>;
}

class TodoApp extends Component<ITodoAppProps> {
  public render() {
    const todos = this.props.todos.filter(
      todo => todo.userId === this.props.selectedUser
    );
    todos.sort((a, b) => b.id.localeCompare(a.id));

    const todoItems = todos.map(todo => (
      <TodoRow
        key={todo.id}
        todo={todo}
        onTodoModified={this.onTodoModified}
        onTodoDeleted={this.onTodoDeleted}
      />
    ));

    const users = this.props.users;

    const pages =
      users &&
      users.map(user => (
        <Pagination.Item
          key={user.id}
          active={user.id === this.props.selectedUser}
          onClick={() => this.selectPage(user.id)}
        >
          {user.name}
        </Pagination.Item>
      ));

    const buttonContent = this.props.isFetchingTodos ? (
      <div>
        <Spinner
          as="span"
          animation="grow"
          size="sm"
          role="status"
          aria-hidden="true"
        />
        Loading todos...
      </div>
    ) : (
      <span>Add new Todo</span>
    );

    const navbarContent = this.props.isFetchingUsers ? (
      <div>
        <Spinner
          as="span"
          animation="grow"
          size="sm"
          role="status"
          aria-hidden="true"
        />
        Loading users...
      </div>
    ) : (
      <Pagination className="justify-content-end">{pages}</Pagination>
    );

    return (
      <div>
        <Navbar bg="primary" variant="dark">
          <Navbar.Brand>Todo App</Navbar.Brand>
          <Button
            variant="outline-light"
            onClick={this.addTodoClicked}
            disabled={this.props.isFetchingTodos}
          >
            {buttonContent}
          </Button>
        </Navbar>
        {navbarContent}
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>User</th>
              <th>#</th>
              <th>Task</th>
              <th>Done</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>{todoItems}</tbody>
        </Table>
        <AddTodoModal />
      </div>
    );
  }

  private addTodoClicked = () => this.props.dispatch(showAddTodoModal());

  private selectPage(selected: number) {
    this.props.dispatch(selectUser(selected));
  }

  private onTodoModified = (todo: ITodo) =>
    this.props.dispatch(modifyTodo(todo));

  private onTodoDeleted = (id: string) => this.props.dispatch(deleteTodo(id));
}

const mapStateToProps = (store: ITodoState) => store;

export default connect(mapStateToProps)(TodoApp);
