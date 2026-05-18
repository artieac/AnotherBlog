import RestClient from './RestClient';
import { IUser } from '@/types/user.types';

class UserRepository {
    public async getAll(): Promise<IUser[]> {
        try {
            return await RestClient.get<IUser[]>('/api/Users');
        } catch (error) {
            console.error('Failed to fetch users:', error);
            throw new Error('Could not retrieve users.');
        }
    }

    public async getById(id: number): Promise<IUser> {
        try {
            return await RestClient.get<IUser>(`/api/Users/${id}`);
        } catch (error) {
            console.error(`Failed to fetch user ${id}:`, error);
            throw new Error('Could not retrieve user details.');
        }
    }

    public async getCurrent(): Promise<IUser> {
        try {
            return await RestClient.get<IUser>('/api/Users/Current');
        } catch (error) {
            console.error('Failed to fetch current user:', error);
            throw new Error('Could not retrieve current user information.');
        }
    }

    public async save(user: IUser): Promise<IUser> {
        try {
            return await RestClient.post<IUser>(`/api/Users/${user.Id}`, user);
        } catch (error) {
            console.error(`Failed to save user ${user.Id}:`, error);
            throw new Error('Could not save user information.');
        }
    }

    public async delete(id: number): Promise<void> {
        try {
            return await RestClient.delete<void>(`/api/Users/${id}`);
        } catch (error) {
            console.error(`Failed to delete user ${id}:`, error);
            throw new Error('Could not delete the user.');
        }
    }
}

export default new UserRepository();

