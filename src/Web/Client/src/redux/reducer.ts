import { ITodo } from "../model/todo";
import {
  CLOSE_ADD_TODO_MODAL,
  END_POST_TODO,
  ITodoAction,
  RECEIVE_TODOS,
  REQUEST_TODOS,
  SELECT_USER,
  SHOW_ADD_TODO_MODAL,
  START_POST_TODO,
  UPDATE_PENDING_TODO,
  START_MODIFY_TODO,
  END_MODIFY_TODO,
  END_DELETE_TODO
} from "./actions";

export interface ITodoState {
  isFetching: boolean;
  lastUpdated: Date;
  todos: ITodo[];
  selectedUser: number;
  showAddTodoModal: boolean;
  pendingTodo: ITodo;
}

const emptyPendingTodo = {
  completed: false,
  id: "",
  isChanging: false,
  local: false,
  title: "",
  userId: 1
};

const initialState = {
  isFetching: false,
  lastUpdated: new Date(0),
  pendingTodo: emptyPendingTodo,
  selectedUser: 1,
  showAddTodoModal: false,
  todos: []
};

export function todos(state: ITodoState = initialState, action: ITodoAction) {
  switch (action.type) {
    case REQUEST_TODOS:
      return {
        ...state,
        isFetching: true
      };
    case RECEIVE_TODOS:
      return {
        ...state,
        isFetching: false,
        lastUpdated: action.receivedAt,
        todos: action.todos
      };
    case SHOW_ADD_TODO_MODAL:
      return {
        ...state,
        showAddTodoModal: true
      };
    case CLOSE_ADD_TODO_MODAL:
      return {
        ...state,
        showAddTodoModal: false
      };
    case UPDATE_PENDING_TODO:
      return {
        ...state,
        pendingTodo: {
          ...state.pendingTodo,
          title: action.title,
          userId: action.userId
        }
      };
    case START_POST_TODO:
      return {
        ...state,
        isFetching: true,
        showAddTodoModal: false
      };
    case END_POST_TODO:
      return {
        ...state,
        isFetching: false,
        pendingTodo: {
          ...emptyPendingTodo,
          userId: state.selectedUser
        },
        todos: state.todos.concat([
          {
            ...action.todo,
            local: true
          }
        ])
      };
    case SELECT_USER:
      return {
        ...state,
        pendingTodo: {
          ...state.pendingTodo,
          userId: action.user
        },
        selectedUser: action.user
      };
    case START_MODIFY_TODO:
      const changingTodo = state.todos.find(todo => todo.id === action.id);
      if (!changingTodo) {
        return state;
      }

      changingTodo.isChanging = true;
      return {
        ...state,
        todos: state.todos
          .filter(todo => todo.id !== action.id)
          .concat([changingTodo])
      };
    case END_MODIFY_TODO:
      return {
        ...state,
        todos: state.todos
          .filter(todo => todo.id !== action.todo.id)
          .concat([{ ...action.todo, isChanging: false }])
      };
    case END_DELETE_TODO:
      return {
        ...state,
        todos: state.todos.filter(todo => todo.id !== action.id)
      };
    default:
      return state;
  }
}
