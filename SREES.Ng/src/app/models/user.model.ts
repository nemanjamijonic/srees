import { Role } from './role.enum';

export interface User {
  id: number;
  guid: string;
  firstName: string | null;
  lastName: string | null;
  email: string | null;
  role: Role;
  isDeleted: boolean;
  createdAt: string;
  lastUpdateTime: string;
}

export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: Role;
}

export interface UpdateUserRequest {
  firstName?: string;
  lastName?: string;
  email?: string;
  role?: Role;
}
