import RestClient from './RestClient';
import { IUser } from '../Models/IUser';

class UserRepository {
    public async getAll(): Promise<IUser[]> {
        return RestClient.get<IUser[]>('/api/Users');
    }

    public async getById(id: number): Promise<IUser> {
        return RestClient.get<IUser>(`/api/Users/${id}`);
    }

    public async save(user: IUser): Promise<IUser> {
        return RestClient.post<IUser>(`/api/Users/${user.Id}`, user);
    }

    public async delete(id: number): Promise<void> {
        return RestClient.delete<void>(`/api/Users/${id}`);
    }
}

export default new UserRepository();
