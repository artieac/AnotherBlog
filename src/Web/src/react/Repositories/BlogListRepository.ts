import RestClient from './RestClient';
import { IBlogList, IBlogListItem } from '../Models/IBlogList';

class BlogListRepository {
    public async getByBlog(blogSubFolder: string): Promise<IBlogList[]> {
        return RestClient.get<IBlogList[]>(`/api/Blog/${blogSubFolder}/Lists`);
    }

    public async getById(blogSubFolder: string, id: number): Promise<IBlogList> {
        return RestClient.get<IBlogList>(`/api/Blog/${blogSubFolder}/List/${id}`);
    }

    public async save(blogSubFolder: string, list: IBlogList): Promise<IBlogList> {
        const input = {
            Name: list.Name,
            ShowOrdered: list.ShowOrdered
        };

        if (list.Id && list.Id > 0) {
            return RestClient.put<IBlogList>(`/api/Blog/${blogSubFolder}/List/${list.Id}`, input);
        } else {
            return RestClient.post<IBlogList>(`/api/Blog/${blogSubFolder}/List`, input);
        }
    }

    public async delete(blogSubFolder: string, id: number): Promise<boolean> {
        return RestClient.delete<boolean>(`/api/Blog/${blogSubFolder}/List/${id}`);
    }

    public async saveItem(blogSubFolder: string, listId: number, item: IBlogListItem): Promise<IBlogList> {
        const input = {
            Name: item.Name,
            RelatedLink: item.RelatedLink,
            DisplayOrder: item.DisplayOrder
        };

        if (item.Id && item.Id > 0) {
            return RestClient.put<IBlogList>(`/api/Blog/${blogSubFolder}/List/${listId}/Item/${item.Id}`, input);
        } else {
            return RestClient.post<IBlogList>(`/api/Blog/${blogSubFolder}/List/${listId}/Item`, input);
        }
    }

    public async deleteItem(blogSubFolder: string, listId: number, itemId: number): Promise<IBlogList> {
        return RestClient.delete<IBlogList>(`/api/Blog/${blogSubFolder}/List/${listId}/Item/${itemId}`);
    }
}

export default new BlogListRepository();
